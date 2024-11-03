using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BakGame.Utils;

public class BossAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    [SerializeField] private bool isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMultiplier = 2f;

    [SerializeField] public bool _isAttackingEnemy = false;
    [SerializeField] private float _attackingDistance = 2f;
    [SerializeField] private float _attackRate = 1f;
    private float _nextAttackTime = 0f;

    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private Transform fireBallSpawnPoint;
    [SerializeField] private float fireBallSpeed = 10f;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRange = 2f;
    [SerializeField] private float spawnRate = 5f;
    private float _nextSpawnTime = 0f;

    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float shieldRadius = 2f;
    [SerializeField] private float shieldDuration = 5f;
    private GameObject _activeShield;
    private float _shieldStartTime;

    private NavMeshAgent navMeshAgent;
    private State _currentState;
    private float roamingTime;
    private Vector3 roamPosition;
    private Vector3 startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    private float _nextCheckDirectionTime = 0f;
    private float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;

    public event EventHandler OnEnemyAttack;
    public event EventHandler OnEnemySpawn;
    public event EventHandler OnEnemyShieldActivated;

    public bool IsRunning
    {
        get
        {
            if (navMeshAgent.velocity == Vector3.zero)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        _currentState = startingState;

        _roamingSpeed = navMeshAgent.speed;
        _chasingSpeed = navMeshAgent.speed * _chasingSpeedMultiplier;

        startingPosition = transform.position;
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }

    public void SetDeathState()
    {
        navMeshAgent.ResetPath();
        _currentState = State.Death;
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            default:
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0)
                {
                    Roaming();
                    roamingTime = roamingTimerMax;
                }
                CheckCurrentState();
                break;

            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            case State.Idle:
                break;
        }
    }

    private void ChasingTarget()
    {
        navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    public float GetRoamingAnimationSpeed()
    {
        return navMeshAgent.speed / _roamingSpeed;
    }

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (isChasingEnemy)
        {
            if (distanceToPlayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (_isAttackingEnemy)
        {
            if (distanceToPlayer <= _attackingDistance)
            {
                newState = State.Attacking;
            }
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                roamingTime = 0f;
                navMeshAgent.speed = _roamingSpeed;
            }
            else if (newState == State.Attacking)
            {
                navMeshAgent.ResetPath();
            }

            _currentState = newState;
        }

    }

    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate; 
            FireBall(Player.Instance.transform.position); 
        }

        // Призывать врагов
        if (Time.time > _nextSpawnTime)
        {
            OnEnemySpawn?.Invoke(this, EventArgs.Empty);
            _nextSpawnTime = Time.time + spawnRate;
            SpawnEnemy(); 
        }

        if (_activeShield == null) 
        {
            ActivateShield(); 
        }

        if (_activeShield != null && Time.time - _shieldStartTime > shieldDuration)
        {
            Destroy(_activeShield);
            _activeShield = null;
        }
    }

    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning)
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (_currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, Player.Instance.transform.position);
            }

            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }


    private void Roaming()
    {
        roamPosition = GoRoamingPosition();
        navMeshAgent.SetDestination(roamPosition);
        ChangeFacingDirection(startingPosition, roamPosition);
    }

    private Vector3 GoRoamingPosition()
    {
        Vector3 newRoamingPosition = startingPosition;                   
                
        for (int i = 0; i < 10; i++)                   
        {
            newRoamingPosition = startingPosition + BakUtils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);

                                                 
            if (Vector3.Distance(newRoamingPosition, startingPosition) > roamingDistanceMin)               
            {
                break;                                             
            }
        }

        return newRoamingPosition;
    }


    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void UpdatePath()
    {
        if (_currentState == State.Chasing)
        {
            navMeshAgent.SetDestination(Player.Instance.transform.position);
        }
        else if (_currentState == State.Roaming)
        {
            navMeshAgent.SetDestination(roamPosition);
        }
    }

    private void FireBall(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - fireBallSpawnPoint.position).normalized;

        GameObject fireBall = Instantiate(fireBallPrefab, fireBallSpawnPoint.position, Quaternion.identity);

        fireBall.GetComponent<Rigidbody2D>().velocity = direction * fireBallSpeed;
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnOffset = new Vector3(
                UnityEngine.Random.insideUnitCircle.x,
                UnityEngine.Random.insideUnitCircle.y,
                0f 
            );

            spawnOffset *= spawnRange;

            Vector3 spawnPosition = transform.position + spawnOffset;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void ActivateShield()
    {
        if (_activeShield == null)
        {
            CircleCollider2D bossCollider = GetComponent<CircleCollider2D>();

            Vector3 shieldPosition = transform.position;

            _activeShield = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity);
            _activeShield.transform.localScale = new Vector3(shieldRadius, shieldRadius, 1);

            _shieldStartTime = Time.time;
            OnEnemyShieldActivated?.Invoke(this, EventArgs.Empty);
        }
    }
}


