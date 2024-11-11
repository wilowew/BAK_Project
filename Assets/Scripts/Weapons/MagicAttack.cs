using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    [SerializeField] private GameObject magicProjectilePrefab; 
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float fireCooldown = 0.5f; // Время кулдауна между выстрелами

    private float lastFireTime; // Время последнего выстрела
    private int reload = 0;

    void Update()
    {
        Debug.Log(reload);
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= lastFireTime + fireCooldown)
        {
            ShootMagic();
            lastFireTime = Time.time; // Обновляем время последнего выстрела
        }
        if (reload == 5)
        {
            lastFireTime += 4f;
            reload = 0;
        }
        if (Time.time >= lastFireTime + 2.5f && reload > 0)
        {
            reload--;
            lastFireTime = Time.time;
        }
    }

    private void ShootMagic()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - firePoint.position).normalized;

        GameObject projectile = Instantiate(magicProjectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;
        reload++;
    }
}
