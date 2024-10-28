using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BakGame.Utils;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    [SerializeField] private bool isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMultiplier = 2f;

    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackingDistance = 2f;
    [SerializeField] private float _attackRate = 2f;
    private float _nextAttackTime = 0f;

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

        // Устанавливаем стартовую позицию врага
        startingPosition = transform.position;

        _roamingSpeed = navMeshAgent.speed;
        _chasingSpeed = navMeshAgent.speed * _chasingSpeedMultiplier;
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
        if (_currentState == State.Chasing)
        {
            navMeshAgent.SetDestination(Player.Instance.transform.position);
        }
    }

    private void SetChasingState()
    {
        if (_currentState != State.Chasing)
        {
            _currentState = State.Chasing;
            navMeshAgent.speed = _chasingSpeed;
            navMeshAgent.ResetPath();
        }
    }

    private void SetAttackingState()
    {
        if (_currentState != State.Attacking)
        {
            _currentState = State.Attacking;
            navMeshAgent.ResetPath();
        }
    }

    private void SetRoamingState()
    {
        if (_currentState != State.Roaming)
        {
            _currentState = State.Roaming;
            navMeshAgent.speed = _roamingSpeed;
            roamingTime = 0f;
        }
    }

    private bool CanSeePlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.Instance.transform.position - transform.position);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    public float GetRoamingAnimationSpeed()
    {
        return navMeshAgent.speed / _roamingSpeed;
    }

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (isChasingEnemy && distanceToPlayer <= _chasingDistance)
        {
            SetChasingState();
        }
        else if (_isAttackingEnemy && distanceToPlayer <= _attackingDistance)
        {
            SetAttackingState();
        }
        else if (distanceToPlayer > _chasingDistance)
        {
            SetRoamingState();
        }
    }

    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
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
        return startingPosition + BakUtils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
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
}