using System.Collections;
using UnityEngine;

public class BossHealthRegeneration : MonoBehaviour {
    [SerializeField] private int regenerationAmount = 5; // ������� �������� ����������������� �� ���
    [SerializeField] private float regenerationInterval = 2f; // �������� ����� ��������������� ��������

    private EnemyEntity bossEntity; // ������ �� �������� �����

    private void Start() {
        // �������� ��������� EnemyEntity �� ���� �������
        bossEntity = GetComponent<EnemyEntity>();

        // ���� ���� ������, ��������� �������� �������������� ��������
        if (bossEntity != null) {
            StartCoroutine(RegenerateHealth());
        }
        else {
            Debug.LogWarning("Boss does not have an EnemyEntity component attached!");
        }
    }

    private IEnumerator RegenerateHealth() {
        // ���� �� ����� ���� ������� � ����� "Gribok", ���������� ��������� ��������
        while (AreGribokObjectsPresent()) {
            // ���������, ����� �� ������������ �������� (�� ������ ���������)
            if (bossEntity.CurrentHealth < bossEntity.MaxHealth) {
                // ��������� �������� ����� (�������� ������������� �������� ��� ��������������)
                bossEntity.TakeDamage(-regenerationAmount); // ������� �����, ����� ������������ ��������
            }

            // ���� ��������� ����� ����� ��������� ����������� ��������
            yield return new WaitForSeconds(regenerationInterval);
        }
    }

    private bool AreGribokObjectsPresent() {
        // ���� ��� ������� � ����� "Gribok" � �����
        GameObject[] griboks = GameObject.FindGameObjectsWithTag("Gribok");

        // ���� ���� �� ���� ������ � ����� ����� ������, ���������� true
        return griboks.Length > 0;
    }
}
