using UnityEngine;

public class BossTrigger : MonoBehaviour {
    [Header("Boss Settings")]
    [SerializeField] private GameObject bossPrefab; // Префаб босса
    [SerializeField] private Vector3 spawnPosition = new Vector3(0, 0, 0); // Координаты для спавна босса

    private bool hasSpawned = false;

    void OnTriggerEnter2D(Collider2D other) {
        // Проверяем, что в триггер вошел игрок
        if (other.CompareTag("Player") && !hasSpawned) {
            SpawnBoss();
            hasSpawned = true;
        }
    }

    void SpawnBoss() {
        // Спавним босса в заданных координатах
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    }
}
