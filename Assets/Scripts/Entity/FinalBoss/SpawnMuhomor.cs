using UnityEngine;

public class BossTrigger : MonoBehaviour {
    [Header("Boss Settings")]
    [SerializeField] private GameObject bossPrefab; // Префаб босса
    [SerializeField] private Vector3 spawnPosition = new Vector3(0, 0, 0); // Координаты для спавна босса
    [SerializeField] private Door door; // Ссылка на объект двери
    [SerializeField] private GameObject polosaGovna; // Ссылка на объект с тегом PolosaGovna

    private bool hasSpawned = false;

    void OnTriggerEnter2D(Collider2D other) {
        // Проверяем, что в триггер вошел игрок и босс еще не был спавнен
        if (other.CompareTag("Player") && !hasSpawned) {
            SpawnBoss();
            hasSpawned = true;

            // Закрываем дверь, если она была открыта
            if (door != null && !door.IsLocked()) {
                door.CloseDoor();
            }

            // Включаем объект с тегом PolosaGovna
            if (polosaGovna != null) {
                polosaGovna.SetActive(true);
            }
        }
    }

    void SpawnBoss() {
        // Спавним босса в заданных координатах
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    }
}
