using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BakGame.Utils;

public class BossAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
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

    [SerializeField] private float damageThreshold = 30f;
    [SerializeField] private float damageTimerThreshold = 5f;
    [SerializeField] private float knockbackForce = 7f;

    private List<Vector3> lastTeleportPositions = new List<Vector3>();
    private int maxTeleportHistory = 1; 

    [Header("Music Players")]
    [SerializeField] private MusicPlayer activeMusicPlayer; 
    [SerializeField] private MusicPlayer MusicPlayerAfterDeath;

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

    private float currentDamage = 0f;
    private float damageTimer = 0f;

    public event EventHandler OnEnemyAttack;
    public event EventHandler OnEnemySpawn;

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
        _currentState = _startingState;

        _roamingSpeed = navMeshAgent.speed;
        _chasingSpeed = navMeshAgent.speed * _chasingSpeedMultiplier;

        startingPosition = transform.position;
        _nextTeleportTime = Time.time + _teleportCooldown;
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();

        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
        else
        {
            currentDamage = 0f;
        }
    }

    public bool IsRunning
    {
        get
        {
            return navMeshAgent.velocity != Vector3.zero;
        }

    }

    public void TakeDamage(float damage)
    {
        currentDamage += damage;
        damageTimer = damageTimerThreshold;

        if (currentDamage >= damageThreshold)
        {
            KnockbackPlayer();
            currentDamage = 0f;
            damageTimer = 0f;
        }
    }

    public float GetRoamingAnimationSpeed()
    {
        return navMeshAgent.speed / _roamingSpeed;
    }

    private void DeathChangeMusic()
    {
        if (activeMusicPlayer != null)
        {
            Debug.Log($"Disabling current MusicPlayer: {activeMusicPlayer.name}");
            activeMusicPlayer.PauseMusic();
            activeMusicPlayer.enabled = false; 
            activeMusicPlayer.gameObject.SetActive(false); 
        }

        if (MusicPlayerAfterDeath != null)
        {
            MusicPlayerAfterDeath.gameObject.SetActive(true); 
            MusicPlayerAfterDeath.enabled = true;
            Debug.Log($"Enabling new MusicPlayer: {MusicPlayerAfterDeath.name}");
            MusicPlayerAfterDeath.Initialize(); 
            MusicPlayerAfterDeath.ResumeMusic();
        }
    }

    public void SetDeathState()
    {
        DeathChangeMusic();
        navMeshAgent.ResetPath();
        _currentState = State.Death;
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

    public void OnPlayerStepOnPlate()
    {
        StartCoroutine(TransitionCamera(Player.Instance.transform));
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
                    roamingTime = _roamingTimerMax;
                }
                CheckCurrentState();
                break;

            case State.Chasing:
                ChasingTarget();
                float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
                if (_canTeleport && Time.time > _nextTeleportTime && distanceToPlayer <= enemySpawnDistance)
                {
                    int randomChoice = UnityEngine.Random.Range(0, 100);
                    if (randomChoice <= 10)
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

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (distanceToPlayer <= enemySpawnDistance && _canTeleport)
        {
            if (Time.time > _nextTeleportTime)
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
            SwitchState(State.SpawningFireBalls);
        }
        else if (_isChasingEnemy && distanceToPlayer <= _chasingDistance)
        {
            SwitchState(State.Chasing);
        }
        else
        {
            SwitchState(State.Roaming);
        }
    }

    private void KnockbackPlayer()
    {
        Rigidbody2D playerRigidbody = Player.GetInstance().GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;
            playerRigidbody.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void Teleport()
    {
        List<Vector3> teleportCoordinates = new List<Vector3>
        {
            new Vector3(277f, -9f, 0f),
            new Vector3(255f, -19f, 0f),
            new Vector3(283f, -27f, 0f),
        };

        Vector3 chosenTarget;

        do
        {
            int randomIndex = UnityEngine.Random.Range(0, teleportCoordinates.Count);
            chosenTarget = teleportCoordinates[randomIndex];
        } while (lastTeleportPositions.Contains(chosenTarget));

        lastTeleportPositions.Add(chosenTarget);

        if (lastTeleportPositions.Count > maxTeleportHistory)
        {
            lastTeleportPositions.RemoveAt(0);
        }

        _teleportTarget = chosenTarget;
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
            OnEnemySpawn?.Invoke(this, EventArgs.Empty);
            _nextSpawnTime = Time.time + _spawnRate;
            SpawnEnemy();
        }

        if (_currentState == State.SpawningFireBalls && Time.time >= _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;

            float randomValue = UnityEngine.Random.value;
            if (randomValue <= 0.2f)
            {
                FireMultipleBalls(Player.Instance.transform.position);
            }
            else
            {
                FireBall(Player.Instance.transform.position);
            }
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
            newRoamingPosition = startingPosition + BakUtils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);

            if (Vector3.Distance(newRoamingPosition, startingPosition) > _roamingDistanceMin)
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

    private void FireBall(Vector3 targetPosition)
    {
        float offset = 1.5f;
        Vector3 forwardOffset = transform.right * offset;
        Vector3 spawnPosition = transform.position + forwardOffset;
        Vector3 direction = (targetPosition - spawnPosition).normalized;
        GameObject fireBall = Instantiate(fireBallPrefab, spawnPosition, Quaternion.identity);
        fireBall.GetComponent<Rigidbody2D>().velocity = direction * fireBallSpeed;
    }

    private void FireMultipleBalls(Vector3 targetPosition)
    {
        int fireBallCount = 10;
        float angleStep = 36f;
        float currentAngle = 0f;

        for (int i = 0; i < fireBallCount; i++)
        {
            float radianAngle = Mathf.Deg2Rad * currentAngle;
            Vector3 direction = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0);

            Vector3 spawnPosition = transform.position + direction.normalized * 1.5f;
            GameObject fireBall = Instantiate(fireBallPrefab, spawnPosition, Quaternion.identity);
            fireBall.GetComponent<Rigidbody2D>().velocity = direction * fireBallSpeed;

            currentAngle += angleStep;
        }
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

    private IEnumerator TransitionCamera(Transform playerTransform)
    {
        navMeshAgent.isStopped = true;

        Vector3 originalCameraPosition = Camera.main.transform.position;
        Vector3 bossPosition = transform.position;

        float _transitionTime = 5f;
        float _focusTime = 7f;
        float _elapsed = 0f;

        float _originalPlayerSpeed = Player.GetInstance()._movingSpeed;
        Player.GetInstance()._movingSpeed = 0f;

        while (_elapsed < _transitionTime)
        {
            Camera.main.transform.position = Vector3.Lerp(originalCameraPosition, new Vector3(bossPosition.x, bossPosition.y, originalCameraPosition.z), _elapsed / _focusTime);
            _elapsed += Time.deltaTime;
            yield return null;
        }

        Player.GetInstance()._movingSpeed = _originalPlayerSpeed;
        navMeshAgent.isStopped = false;
    }
}
