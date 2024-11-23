using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 2;
    [SerializeField] private float _attackDuration = 0.05f;

    public event EventHandler OnSwordSwing;

    private PolygonCollider2D _polygonCollider2D;
    private bool _isAttacking = false;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        AttackColliderTurnOff();
    }

    private void Start()
    {
        AttackColliderTurnOff();
    }

    public void Attack()
    {
        if (gameObject.activeInHierarchy && !_isAttacking)
        {
            _isAttacking = true;
            OnSwordSwing?.Invoke(this, EventArgs.Empty);
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        AttackColliderTurnOn();
        yield return new WaitForSeconds(_attackDuration);
        AttackColliderTurnOff();
        _isAttacking = false;
    }

    public int GetDamageAmount()
    {
        return _damageAmount;
    }

    public void ResetAttack()
    {
        _isAttacking = false;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void AttackColliderTurnOff() => _polygonCollider2D.enabled = false;

    public void AttackColliderTurnOn() => _polygonCollider2D.enabled = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(_damageAmount);
        }

        if (collision.transform.TryGetComponent(out BossEntity bossEntity))
        {
            bossEntity.TakeDamage(_damageAmount);
        }
    }
}