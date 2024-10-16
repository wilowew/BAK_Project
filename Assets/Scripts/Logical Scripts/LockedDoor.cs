using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
    private Collider2D[] doorColliders; // Массив для всех коллайдеров двери

    private void Awake()
    {
        // Получаем все коллайдеры двери
        doorColliders = GetComponents<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что это игрок и у него есть ключ
        if (collision.CompareTag("Player") && PlayerInventory.Instance.HasKey())
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if (!isOpen)
        {
            Debug.Log("Дверь открыта!");
            isOpen = true;

            // Отключаем все коллайдеры двери
            foreach (var col in doorColliders)
            {
                col.enabled = false;
            }
        }
    }
}