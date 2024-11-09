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
    [SerializeField] private float _attackRate = 1f;
    private float _nextAttackTime = 0f;

    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private float fireBallSpeed = 10f;

    [SerializeField] private float fireBallAttackDistance = 5f;
    [SerializeField] private float enemySpawnDistance = 2f;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRange = 2f;
    [SerializeField] private float _spawnRate = 5f;
    private float _nextSpawnTime = 0f;

    [SerializeField] private bool _canTeleport = true;
    [SerializeField] private float _teleportCooldown = 30f; 
    private float _nextTeleportTime = 0f;
    private Vector3 _teleportTarget;

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

    public bool IsRunning
    {
        get
        {
            return navMeshAgent.velocity != Vector3.zero;
        }
    }

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Teleporting,
        SpawningFireBalls,
        SpawningEnemies,
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
        _nextTeleportTime = Time.time; 
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

        PressurePlateBoss plate = FindObjectOfType<PressurePlateBoss>();
        if (plate != null)
        {
            plate.StopMusic();
        }
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
                float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
                if (_canTeleport && Time.time > _nextTeleportTime && distanceToPlayer <= enemySpawnDistance)
                {
                    int randomChoice = UnityEngine.Random.Range(0, 2);
                    if (randomChoice == 0)
                    {
                        SwitchState(State.Teleporting);
                    }
                    else
                    {
                        SwitchState(State.SpawningEnemies);
                    }
                }
                CheckCurrentState();
                break;

            case State.Teleporting:
                Teleport();
                SwitchState(State.Chasing);
                break;
            case State.SpawningFireBalls:
            case State.SpawningEnemies:
                AttackingTarget();
                SwitchState(State.Chasing);
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

        Debug.Log($"Distance to player: {distanceToPlayer}");

        if (distanceToPlayer <= enemySpawnDistance && _canTeleport)
        {
            Debug.Log("Switching to State: Teleporting or Spawning Enemies");
            int randomChoice = UnityEngine.Random.Range(0, 2);
            if (randomChoice == 0)
            {
                SwitchState(State.Teleporting);
            }
            else
            {
                SwitchState(State.SpawningEnemies);
            }
        }
        else if (distanceToPlayer <= fireBallAttackDistance)
        {
            Debug.Log("Switching to Spawning FireBalls State");
            SwitchState(State.SpawningFireBalls);
        }
        else if (isChasingEnemy && distanceToPlayer <= _chasingDistance)
        {
            Debug.Log("Switching to Chasing State");
            SwitchState(State.Chasing);
        }
        else
        {
            Debug.Log("Switching to Roaming State");
            SwitchState(State.Roaming);
        }
    }

    private void Teleport()
    {
        List<Vector3> teleportCoordinates = new List<Vector3>
        {
            new Vector3(277f, -5f, 0f),
            new Vector3(255f, -18f, 0f),
            new Vector3(283f, -24f, 0f),
        };

        int randomIndex = UnityEngine.Random.Range(0, teleportCoordinates.Count);
        _teleportTarget = teleportCoordinates[randomIndex];

        transform.position = _teleportTarget;
        _nextTeleportTime = Time.time + _teleportCooldown; 
    }

    private Vector3 GetTeleportPosition()
    {
        Vector3 randomOffset = UnityEngine.Random.insideUnitCircle * 3f;
        Vector3 target = Player.Instance.transform.position + randomOffset;
        return target;
    }

    private void SwitchState(State newState)
    {
        if (newState != _currentState)
        {
            switch (newState)
            {
                case State.Chasing:
                    navMeshAgent.ResetPath();
                    navMeshAgent.speed = _chasingSpeed;
                    break;
                case State.Roaming:
                    roamingTime = 0f;
                    navMeshAgent.speed = _roamingSpeed;
                    break;
                case State.SpawningFireBalls:
                case State.SpawningEnemies:
                case State.Attacking:
                    navMeshAgent.ResetPath();
                    break;
            }
            _currentState = newState;
        }
    }

    private void AttackingTarget()
    {
        if (_currentState == State.SpawningEnemies && Time.time >= _nextSpawnTime)
        {
            Debug.Log("Spawning enemies!");
            OnEnemySpawn?.Invoke(this, EventArgs.Empty);
            _nextSpawnTime = Time.time + _spawnRate;  
            SpawnEnemy();
        }

        if (_currentState == State.SpawningFireBalls && Time.time >= _nextAttackTime)
        {
            Debug.Log("Attacking with FireBall!");
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
            FireBall(Player.Instance.transform.position);
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
            else if (_currentState == State.SpawningFireBalls || _currentState == State.SpawningEnemies)
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
        float offset = 1.5f;
        Vector3 forwardOffset = transform.right * offset;
        Vector3 spawnPosition = transform.position + forwardOffset;
        Vector3 direction = (targetPosition - spawnPosition).normalized;
        GameObject fireBall = Instantiate(fireBallPrefab, spawnPosition, Quaternion.identity);
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

    public void OnPlayerStepOnPlate()
    {
        StartCoroutine(TransitionCamera(Player.Instance.transform));
    }

    private IEnumerator TransitionCamera(Transform playerTransform)
    {
        Vector3 originalCameraPosition = Camera.main.transform.position;
        Vector3 bossPosition = transform.position;

        float transitionTime = 5f;
        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            Camera.main.transform.position = Vector3.Lerp(originalCameraPosition, new Vector3(bossPosition.x, bossPosition.y, originalCameraPosition.z), elapsed / transitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        BossHealthBarController bossBar = FindObjectOfType<BossHealthBarController>();
        if (bossBar != null)
        {
            bossBar.gameObject.SetActive(true);
        }

        elapsed = 0f;
        while (elapsed < transitionTime)
        {
            Camera.main.transform.position = Vector3.Lerp(bossPosition, new Vector3(playerTransform.position.x, playerTransform.position.y, originalCameraPosition.z), elapsed / transitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
