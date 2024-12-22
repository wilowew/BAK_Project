using UnityEngine;

public class BossEnemySpawner : MonoBehaviour {
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject enemyPrefab1;  // Префаб врага 1
    [SerializeField] private GameObject enemyPrefab2;  // Префаб врага 2
    [SerializeField] private GameObject enemyPrefab3;  // Префаб врага 3

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 5f;    // Радиус, в котором будут появляться враги
    [SerializeField] private float spawnInterval = 3f;  // Интервал времени между спавнами (после первого)
    [SerializeField] private int enemiesPerSpawn = 3;   // Количество врагов, которые будут спавниться за раз
    [SerializeField] private float firstSpawnDelay = 2f; // Задержка перед первым спавном

    void Start() {
        // Используем Invoke с задержкой для первого спавна
        Invoke("StartSpawning", firstSpawnDelay);
    }

    void StartSpawning() {
        // Начинаем спавнить врагов через заданный интервал (после задержки)
        InvokeRepeating("SpawnEnemies", 0f, spawnInterval);
    }

    void SpawnEnemies() {
        // Спавним несколько врагов за раз
        for (int i = 0; i < enemiesPerSpawn; i++) {
            // Выбираем случайный префаб
            GameObject enemyPrefab = ChooseRandomEnemyPrefab();

            // Выбираем случайную позицию вокруг босса
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

            // Спавним врага на выбранной позиции
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Метод для случайного выбора префаба врага
    GameObject ChooseRandomEnemyPrefab() {
        int randomIndex = Random.Range(0, 3);

        switch (randomIndex) {
            case 0:
                return enemyPrefab1;
            case 1:
                return enemyPrefab2;
            case 2:
                return enemyPrefab3;
            default:
                return enemyPrefab1;  // Возвращаем по умолчанию префаб 1
        }
    }
}
