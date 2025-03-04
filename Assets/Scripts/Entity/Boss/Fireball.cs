using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 3;

    private void Start()
    {
        Destroy(gameObject, 1.2f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(transform, _damageAmount);
            Destroy(gameObject);
        }

        if (!(collider.CompareTag("Player") || (collider.CompareTag("Item")) || (collider.CompareTag("Projectile"))))
        {
            Destroy(gameObject);
        }
    }
}
