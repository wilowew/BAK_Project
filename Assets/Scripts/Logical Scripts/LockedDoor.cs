using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isLockedByKey = true;
    private NavMeshObstacle navMeshObstacle;

    private void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        if (navMeshObstacle == null)
        {
            Debug.LogWarning("NavMeshObstacle component is missing on the door.");
        }
    }

    public void UnlockDoor()
    {
        isLockedByKey = false;
    }

    public bool IsLocked()
    {
        return isLockedByKey;
    }

    private void OpenDoor()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (navMeshObstacle != null)
        {
            Destroy(navMeshObstacle); // Полностью удаляем компонент, чтобы убедиться, что он не мешает
        }

        // Принудительно обновляем путь для всех ботов
        UpdateEnemyPaths();

        Debug.Log("Door opened, all colliders removed, and NavMeshObstacle carving disabled.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isLockedByKey)
        {
            OpenDoor();
        }
    }

    // Метод для обновления путей всех врагов
    private void UpdateEnemyPaths()
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>(); // Находим всех врагов в сцене
        foreach (var enemy in enemies)
        {
            enemy.UpdatePath(); // Обновляем их путь
        }
    }
}