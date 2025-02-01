using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerHurt;

    [SerializeField] public float _movingSpeed = 7f;
    [SerializeField] public int _maxHealth = 20;
    [SerializeField] private float _healthRegenRate = 1f;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    private float _minMovingSpeed = 0.1f;
    private float _MovingSpeedDebuff = 1f;
    private bool _isRunning = false;
    private bool _isAttacking = false;

    public int _currentHealth;
    private bool _canTakeDamage;
    private float _lastRegenTime;
    private bool _isAlive;

    private Coroutine currentSpeedBoostCoroutine;

    public int _coins { get; private set; } = 0;

    Vector2 inputVector;

    public int CurrentHealth { get { return _currentHealth; } }
    public int MaxHealth { get { return _maxHealth; } }
    public int LastCollectedCoinAmount { get; private set; }

    public event EventHandler AddCoins;


    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _canTakeDamage = true;
        _isAlive = true;
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
    }

    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Update()
    {
        Attacking();
        inputVector = GameInput.Instance.GetMovementVector();

        if (PauseMenu._isPaused) return;

        if (Time.time - _lastRegenTime >= 10f / _healthRegenRate)
        {
            _lastRegenTime = Time.time;
            Heal(1);
        }

    }

    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockedBack)
        {
            return;
        }

        HandleMovement();
    }

    public void Revive()
    {
        _currentHealth = _maxHealth;
        _isAlive = true;

        _canTakeDamage = true;
        GameInput.Instance.EnableMovement();

        _knockBack.StopKnockBackMovement();
        _isRunning = false;
        _isAttacking = false;
        _MovingSpeedDebuff = 1f;

        HealthDisplay healthDisplay = FindObjectOfType<HealthDisplay>();
        if (healthDisplay != null)
        {
            healthDisplay.ShowHealthDisplay();
            healthDisplay.UpdateHealthDisplay();
        }
    }

    public void KillPlayer()
    {

        _currentHealth = 0;
        DetectDeath();
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    public void CollectCoins(int amount)
    {
        LastCollectedCoinAmount = amount;
        _coins += amount;
        AddCoins?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        EnemyAI enemyAI = damageSource.GetComponent<EnemyAI>();
        if (_canTakeDamage && (enemyAI == null || enemyAI._isAttackingEnemy))
        {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            _knockBack.GetKnockedBack(damageSource);

            OnPlayerHurt?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
    }

    public IEnumerator SetZeroSpeedForSeconds(float seconds)
    {
        float originalSpeedDebuff = _MovingSpeedDebuff;
        _MovingSpeedDebuff = 0f;
        yield return new WaitForSeconds(seconds);
        _MovingSpeedDebuff = originalSpeedDebuff;
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }

    public void IncreaseSpeed(float multiplier, float duration)
    {
        if (currentSpeedBoostCoroutine != null)
        {
            StopCoroutine(currentSpeedBoostCoroutine);
        }

        _MovingSpeedDebuff = multiplier;
        currentSpeedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(duration));
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public bool IsAttacking()
    {
        return _isAttacking;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public static Player GetInstance()
    {
        return Instance;
    }

    private IEnumerator SpeedBoostCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        _MovingSpeedDebuff = 1f;

        currentSpeedBoostCoroutine = null;
    }

    private void DetectDeath()
    {
        if (_currentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            _knockBack.StopKnockBackMovement();
            GameInput.Instance.DisableMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void HandleMovement()
    {
        if (!_isAlive || PauseMenu._isPaused || _knockBack.IsGettingKnockedBack) return;

        _rb.MovePosition(_rb.position + inputVector * (_movingSpeed * _MovingSpeedDebuff * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

    private void Attacking()
    {
        if (!PauseMenu._isPaused && Input.GetKeyDown(KeyCode.E))
        {
            _isAttacking = !_isAttacking;
            if (_isAttacking == true)
            {
                _MovingSpeedDebuff = 0.65f;
            }
            else
            {
                _MovingSpeedDebuff = 1f;
            }
        }
    }
}