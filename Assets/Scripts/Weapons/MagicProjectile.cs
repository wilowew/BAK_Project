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
            EnemyEntity enemyEntity = collider.GetComponent<EnemyEntity>(); // Предполагаем, что у врага есть компонент Enemy
            if (enemyEntity != null)
            {
                enemyEntity.TakeDamage(_damageAmount);
            }
            Destroy(gameObject); // Уничтожаем снаряд после попадания
        }
        if (collider.CompareTag("Boss") && collider is BoxCollider2D)
        {
            BossEntity bossEntity = collider.GetComponent<BossEntity>(); // Предполагаем, что у врага есть компонент Enemy
            if (bossEntity != null)
            {
                bossEntity.TakeDamage(_damageAmount);
            }
            Destroy(gameObject); // Уничтожаем снаряд после попадания
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
