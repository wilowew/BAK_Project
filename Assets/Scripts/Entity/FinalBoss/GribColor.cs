using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyColorChange : MonoBehaviour {
    [Header("Enemy Settings")]
    [SerializeField] private EnemyEntity enemyEntity;  // ������ �� ��������� EnemyEntity
    [SerializeField] private float cycleSpeed = 1f;  // ������� ��������� ������ S (�� ��� �������)
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyEntity == null) {
            enemyEntity = GetComponent<EnemyEntity>();  // ���� �� �������� �������, �� ���������� ����� �������������
        }
    }

    private void Update() {
        if (enemyEntity != null) {
            UpdateEnemyColor();
        }
    }

    private void UpdateEnemyColor() {
        // ������������ ������� ��������
        float healthPercentage = (float)enemyEntity.CurrentHealth / enemyEntity.MaxHealth;
        // ��������� �������� ��������� ������� �� 0.3 (�������) �� 1 (��������)
        float brightness = Mathf.Lerp(0.3f, 1f, healthPercentage);

        // ����������� ������� ���� � HSV
        Color currentColor = spriteRenderer.color;
        Color.RGBToHSV(currentColor, out float h, out float s, out float v);

        // ���������� �������� S �� 0 �� 0.5
        s = Mathf.PingPong(Time.time * cycleSpeed, 0.5f);  // �� 0 �� 0.5 � ��������, ��������� �� cycleSpeed

        // �� ��������� H ��� ����, ����� ��������� �������� �������.
        // ��������� ����� ���� � ���������� S � �������� (V)
        Color newColor = Color.HSVToRGB(h, s, brightness);

        // ��������� ����� ����
        spriteRenderer.color = newColor;
    }
}
