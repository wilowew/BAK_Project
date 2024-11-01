using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerHurt;

    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] public int _maxHealth = 20;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private Rigidbody2D rb;
    private KnockBack _knockBack;

    private float minMovingSpeed = 0.1f;
    private float MovingSpeedDebuff = 1f;
    private bool isRunning = false;
    private bool isAttacking = false;

    public int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;

    public int coins = 0;

    Vector2 inputVector;

    public int CurrentHealth { get { return _currentHealth; } }
    public int MaxHealth { get { return _maxHealth; } }

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
    }

    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockedBack)
        {
            return;
        }

        HandleMovement();
    }

    public bool IsAlive()
    {
        return _isAlive;
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
            Time.timeScale = 0f;
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    public event EventHandler AddCoins;

    public void CollectCoins(int amount)
    {
        coins += amount;
        AddCoins?.Invoke(this, EventArgs.Empty);
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