using UnityEngine;

public class BossTrigger : MonoBehaviour {
    [Header("Boss Settings")]
    [SerializeField] private GameObject bossPrefab; // Префаб босса
    [SerializeField] private Vector3 spawnPosition = new Vector3(0, 0, 0); // Позиция для спавна босса
    [SerializeField] private Door door; // Ссылка на объект двери, который будет закрываться при активации босса

    [Header("Health Bar Settings")]
    [SerializeField] private Vector3 healthBarNewPosition = new Vector3(0, 850, 0); // Новые координаты для полосы здоровья

    private bool hasSpawned = false;

    void OnTriggerEnter2D(Collider2D other) {
        // Проверяем, что в триггер вошел игрок и босс еще не был спавнен
        if (other.CompareTag("Player") && !hasSpawned) {
            SpawnBoss();
            CloseDoor();  // Закрыть дверь при активации босса
            MoveHealthBarToNewPosition();  // Изменить координаты полосы здоровья
            hasSpawned = true;
        }
    }

    void SpawnBoss() {
        // Спавним босса в заданной позиции
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    }

    void CloseDoor() {
        // Если дверь существует, вызываем ее метод CloseDoor
        if (door != null) {
            door.CloseDoor();
            Debug.Log("Door is closed.");
        }
        else {
            Debug.LogWarning("Door is not assigned in the BossTrigger script.");
        }
    }

    void MoveHealthBarToNewPosition() {
        // Находим объект с тегом "PolosaGovna" и изменяем его позицию
        GameObject healthBar = GameObject.FindGameObjectWithTag("PolosaGovna");
        if (healthBar != null) {
            healthBar.transform.position = healthBarNewPosition;  // Устанавливаем новые координаты из инспектора
        }
        else {
            Debug.LogWarning("Health bar object with tag 'PolosaGovna' not found.");
        }
    }
}
