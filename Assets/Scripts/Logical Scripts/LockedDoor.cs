using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isLockedByKey = true; // По умолчанию дверь закрыта

    // Метод для разблокировки двери
    public void UnlockDoor()
    {
        isLockedByKey = false;
    }

    public bool IsLocked()
    {
        return isLockedByKey;
    }

    // Метод для открытия двери и удаления всех коллайдеров
    private void OpenDoor()
    {
        Collider2D[] colliders = GetComponents<Collider2D>(); // Получаем все коллайдеры на объекте двери

        // Проходим по каждому коллайдеру и отключаем его
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        Debug.Log("Door opened and all colliders removed!");
    }

    // Проверяем, может ли игрок открыть дверь при подходе
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isLockedByKey)
        {
            OpenDoor();
        }
    }
}