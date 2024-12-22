using UnityEngine;

public class BossTrigger : MonoBehaviour {
    [Header("Boss Settings")]
    [SerializeField] private GameObject bossPrefab; // ������ �����
    [SerializeField] private Vector3 spawnPosition = new Vector3(0, 0, 0); // ���������� ��� ������ �����

    private bool hasSpawned = false;

    void OnTriggerEnter2D(Collider2D other) {
        // ���������, ��� � ������� ����� �����
        if (other.CompareTag("Player") && !hasSpawned) {
            SpawnBoss();
            hasSpawned = true;
        }
    }

    void SpawnBoss() {
        // ������� ����� � �������� �����������
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    }
}
