using UnityEngine;

public class BossTrigger : MonoBehaviour {
    [Header("Boss Settings")]
    [SerializeField] private GameObject bossPrefab; // ������ �����
    [SerializeField] private Vector3 spawnPosition = new Vector3(0, 0, 0); // ������� ��� ������ �����
    [SerializeField] private Door door; // ������ �� ������ �����, ������� ����� ����������� ��� ��������� �����

    [Header("Health Bar Settings")]
    [SerializeField] private Vector3 healthBarNewPosition = new Vector3(0, 850, 0); // ����� ���������� ��� ������ ��������

    private bool hasSpawned = false;

    void OnTriggerEnter2D(Collider2D other) {
        // ���������, ��� � ������� ����� ����� � ���� ��� �� ��� �������
        if (other.CompareTag("Player") && !hasSpawned) {
            SpawnBoss();
            CloseDoor();  // ������� ����� ��� ��������� �����
            MoveHealthBarToNewPosition();  // �������� ���������� ������ ��������
            hasSpawned = true;
        }
    }

    void SpawnBoss() {
        // ������� ����� � �������� �������
        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    }

    void CloseDoor() {
        // ���� ����� ����������, �������� �� ����� CloseDoor
        if (door != null) {
            door.CloseDoor();
            Debug.Log("Door is closed.");
        }
        else {
            Debug.LogWarning("Door is not assigned in the BossTrigger script.");
        }
    }

    void MoveHealthBarToNewPosition() {
        // ������� ������ � ����� "PolosaGovna" � �������� ��� �������
        GameObject healthBar = GameObject.FindGameObjectWithTag("PolosaGovna");
        if (healthBar != null) {
            healthBar.transform.position = healthBarNewPosition;  // ������������� ����� ���������� �� ����������
        }
        else {
            Debug.LogWarning("Health bar object with tag 'PolosaGovna' not found.");
        }
    }
}
