using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] private int lockPower = 1;
    [SerializeField] private bool isLocked = true;
    [SerializeField] private Sprite activatedSprite;
    private SpriteRenderer spriteRenderer;
    private NavMeshObstacle navMeshObstacle;
    private int unlockPower = 0;

    private void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (navMeshObstacle == null)
        {
            Debug.LogWarning("NavMeshObstacle component is missing on the door.");
        }
    }

    public void increaseUnlockPower()
    {
        unlockPower += 1;
        Debug.Log("Unlock Power: " + unlockPower);
        if (unlockPower >= lockPower)
        {
            UnlockDoor();
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }

    public int CheckLockPower()
    {
        return lockPower;
    }

    public int CheckUnlockPower()
    {
        return unlockPower;
    }

    public void UnlockDoorByPlate()
    {
        unlockPower += 1;
        Debug.Log("Unlock Power:, " + unlockPower);
        if (unlockPower >= lockPower)
        {
            UnlockDoor();
            OpenDoor();
        }
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    private void OpenDoor()
    {
        spriteRenderer.sprite = activatedSprite;
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
        if (collision.CompareTag("Player") && !isLocked && collision is BoxCollider2D)
        {
            Debug.Log("Door Opened");
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