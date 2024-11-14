using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && collider is BoxCollider2D)
        {
            EnemyEntity enemyEntity = collider.GetComponent<EnemyEntity>(); 
            if (enemyEntity != null)
            {
                enemyEntity.TakeDamage(_damageAmount);
            }
        }
        if (collider.CompareTag("Boss") && collider is BoxCollider2D)
        {
            BossEntity bossEntity = collider.GetComponent<BossEntity>(); 
            if (bossEntity != null)
            {
                bossEntity.TakeDamage(_damageAmount);
            }
            Destroy(gameObject);
        }
        if (!(collider.CompareTag("Player") || (collider.CompareTag("Item"))))
        {
            Destroy(gameObject);
        }
    }

    public int GetDamageAmount()
    {
        return _damageAmount;
    }

}
