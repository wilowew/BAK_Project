using UnityEngine;

public class BossEnemySpawner : MonoBehaviour {
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject enemyPrefab1;  // ������ ����� 1
    [SerializeField] private GameObject enemyPrefab2;  // ������ ����� 2
    [SerializeField] private GameObject enemyPrefab3;  // ������ ����� 3

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 5f;    // ������, � ������� ����� ���������� �����
    [SerializeField] private float spawnInterval = 3f;  // �������� ������� ����� �������� (����� �������)
    [SerializeField] private int enemiesPerSpawn = 3;   // ���������� ������, ������� ����� ���������� �� ���
    [SerializeField] private float firstSpawnDelay = 2f; // �������� ����� ������ �������

    void Start() {
        // ���������� Invoke � ��������� ��� ������� ������
        Invoke("StartSpawning", firstSpawnDelay);
    }

    void StartSpawning() {
        // �������� �������� ������ ����� �������� �������� (����� ��������)
        InvokeRepeating("SpawnEnemies", 0f, spawnInterval);
    }

    void SpawnEnemies() {
        // ������� ��������� ������ �� ���
        for (int i = 0; i < enemiesPerSpawn; i++) {
            // �������� ��������� ������
            GameObject enemyPrefab = ChooseRandomEnemyPrefab();

            // �������� ��������� ������� ������ �����
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

            // ������� ����� �� ��������� �������
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // ����� ��� ���������� ������ ������� �����
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
                return enemyPrefab1;  // ���������� �� ��������� ������ 1
        }
    }
}
