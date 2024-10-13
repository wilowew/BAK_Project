using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    public event EventHandler OnTakeHit;

    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    private EnemyAI _enemyAI;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
