using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerHurt;

    [SerializeField] public float movingSpeed = 5f;
    [SerializeField] public int _maxHealth = 20;
    [SerializeField] private float _healthRegenRate = 1f;
    [SerializeField] private float _damageRecoveryTime = 0.5f;
    [SerializeField] private KeyCode teleportKey = KeyCode.T;

    private Rigidbody2D rb;
    private KnockBack _knockBack;

    private float minMovingSpeed = 0.1f;
    private float MovingSpeedDebuff = 1f;
    private bool isRunning = false;
    private bool isAttacking = false;

    public int _currentHealth;
    private bool _canTakeDamage;
    private float _lastRegenTime;
    private bool _isAlive;

    public int coins { get; private set; } = 0;

    Vector2 inputVector;

    public int CurrentHealth { get { return _currentHealth; } }
    public int MaxHealth { get { return _maxHealth; } }
    public int LastCollectedCoinAmount { get; private set; }

    public event EventHandler AddCoins;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
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

        if (Input.GetKeyDown(teleportKey))
        {
            transform.position = new Vector3(240f, -14f, 0f);
        }

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
        isRunning = false;
        isAttacking = false;
        MovingSpeedDebuff = 1f;

        HealthDisplay healthDisplay = FindObjectOfType<HealthDisplay>();
        if (healthDisplay != null)
        {
            healthDisplay.ShowHealthDisplay();
            healthDisplay.UpdateHealthDisplay();
        }
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    public void CollectCoins(int amount)
    {
        LastCollectedCoinAmount = amount;
        coins += amount;
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

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }

    public void IncreaseSpeed(float multiplier, float duration)
    {
        MovingSpeedDebuff = multiplier;
        StartCoroutine(SpeedBoostCoroutine(duration));
    }

    private IEnumerator SpeedBoostCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        MovingSpeedDebuff = 1f;
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

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsAttacking()
    {
        return isAttacking;
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

    private void HandleMovement()
    {
        rb.MovePosition(rb.position + inputVector * (movingSpeed * MovingSpeedDebuff * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void Attacking()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isAttacking = !isAttacking;
            if (isAttacking == true)
            {
                MovingSpeedDebuff = 0.65f;
            }
            else
            {
                MovingSpeedDebuff = 1f;
            }

        }
    }
}