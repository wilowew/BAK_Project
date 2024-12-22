using UnityEngine;
using UnityEngine.UI;

public class FinalBossHealthBarController : MonoBehaviour {
    [SerializeField] private Image healthBarImage;  // ������ �� ����������� ������ ��������
    private EnemyEntity finalBossEntity;  // ������ �� ��������� EnemyEntity ��� �������� ���������� �����

    private bool isFinalBossPresent = false; // ���� ��� ������������ ������������� �����

    private void Update() {
        // ���������, ���� �� ������ � ����� "Muhomor" �� �����
        bool isBossPresent = GameObject.FindGameObjectsWithTag("Muhomor").Length > 0;

        // ���� ���� �������� �� �����
        if (isBossPresent && !isFinalBossPresent) {
            // ������� ����-������ �� �����
            finalBossEntity = FindObjectOfType<EnemyEntity>();  // ������ EnemyEntity �� �����
            if (finalBossEntity != null) {
                isFinalBossPresent = true;  // ������������� ����, ��� ���� ����������
                // ������������� �� �������
                finalBossEntity.OnTakeHit += UpdateFinalBossHealthBar;
                finalBossEntity.OnDeath += HandleBossDeath;  // ������������� �� ������� ������ �����
                UpdateFinalBossHealthBar(this, System.EventArgs.Empty); // ��������� ������ �������� �����
            }
        }
        // ���� ����� ������ ��� �� �����
        else if (!isBossPresent && isFinalBossPresent) {
            // ������� ������ ��������
            SetHealthBarVisibility(false);
            isFinalBossPresent = false; // ��������� ����
        }
    }

    private void UpdateFinalBossHealthBar(object sender, System.EventArgs e) {
        if (finalBossEntity != null && healthBarImage != null) {
            // ������������ ������� ��������
            float healthPercentage = (float)finalBossEntity.CurrentHealth / finalBossEntity.MaxHealth;

            // ������������� fillAmount ������ ��������
            healthBarImage.fillAmount = healthPercentage;
        }
    }

    private void SetHealthBarVisibility(bool isVisible) {
        // ������������� ��������� ������ ��������
        gameObject.SetActive(isVisible);
    }

    private void HandleBossDeath(object sender, System.EventArgs e) {
        // ����� ���� �������, �������� ������ ��������
        SetHealthBarVisibility(false);
    }

    private void OnDestroy() {
        // ������������ �� �������, ���� ���� ���������
        if (finalBossEntity != null) {
            finalBossEntity.OnTakeHit -= UpdateFinalBossHealthBar;
            finalBossEntity.OnDeath -= HandleBossDeath;
        }
    }
}
