using UnityEngine;

public class ColorCycling : MonoBehaviour {
    [Header("Color Cycling Settings")]
    [SerializeField] private float colorChangeSpeed = 1f;  // �������� ����� �����
    private SpriteRenderer spriteRenderer;
    private bool isBossAlive = true;

    private readonly Color initialColor = Color.HSVToRGB(121f / 360f, 38f / 100f, 45f / 100f); // H = 121, S = 38, V = 45

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start() {
        // ������������� ��������� ���� (����� ���� ���)
        spriteRenderer.color = initialColor;
    }

    void Update() {
        // ���������, ���������� �� ������ � ����� "Muhomor" �� �����
        bool bossExists = GameObject.FindWithTag("Muhomor") != null;

        if (!bossExists && isBossAlive) {
            // ���� ����� ��� �� �����, �������� ������� ��������� �����
            isBossAlive = false;
            StartCoroutine(CycleColor());
        }
        else if (bossExists && !isBossAlive) {
            // ���� ���� ����� �� �����, ������������� ���� ��������� �����
            isBossAlive = true;
            StopCoroutine(CycleColor());
            spriteRenderer.color = initialColor;  // ��������������� �������� ���� (����� ���� ���)
        }
    }

    private System.Collections.IEnumerator CycleColor() {
        float hue = initialColor.r;  // ��������� ������� (H) �� ������������ �����

        while (!isBossAlive) {
            // ����������� �������� ������� � ���� (V = 1 - ������������ �������)
            Color color = Color.HSVToRGB(hue, 1f, 1f);  // S = 1, V = 1

            // ��������� ���� � �������
            spriteRenderer.color = color;

            // ����������� ������� (hue) ��� �������� � ���������� �����
            hue += Time.deltaTime * colorChangeSpeed;
            if (hue > 1f) hue = 0f; // ����� hue ��������� 1 (�������), ������������ � 0 (����� �������)

            yield return null; // ���� �� ���������� �����
        }
    }
}
