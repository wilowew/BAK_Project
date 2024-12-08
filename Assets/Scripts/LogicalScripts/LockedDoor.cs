using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] private int lockPower = 1;
    [SerializeField] private bool isLocked = true;
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private Sprite closedSprite;
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

    public void UnlockPowerToZero()
    {
        unlockPower = 0;
    }

    public void UnlockDoor()
    {
        if (isLocked) 
        {
            isLocked = false;
        }
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
            navMeshObstacle.carving = false;
            navMeshObstacle.enabled = false;
        }

        UpdateEnemyPaths();

        Debug.Log("Door opened, all colliders removed, and NavMeshObstacle carving disabled.");
    }

    public void CloseDoor()
    {
        if (!isLocked)
        {
            spriteRenderer.sprite = closedSprite;
            Collider2D[] colliders = GetComponents<Collider2D>();

            foreach (Collider2D col in colliders)
            {
                col.enabled = true;
            }

            if (navMeshObstacle != null)
            {
                navMeshObstacle.carving = true;
                navMeshObstacle.enabled = false;
            }

            UpdateEnemyPaths();
            Debug.Log("Door closed, colliders enabled, and NavMeshObstacle carving enabled.");
            isLocked = !isLocked;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isLocked && collision is BoxCollider2D)
        {
            Debug.Log("Door Opened");
            OpenDoor();
        }
    }

    private void UpdateEnemyPaths()
    {
        StartCoroutine(DelayedPathUpdate());
    }

    private IEnumerator DelayedPathUpdate()
    {
        yield return new WaitForSeconds(0.1f);

        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (var enemy in enemies)
        {
            enemy.UpdatePath();
        }
    }
}