using UnityEngine;

public class EnemyColliderToggle : MonoBehaviour {
    [Header("Colliders")]
    public PolygonCollider2D polygonCollider;  // ������ �� PolygonCollider2D

    [Header("Settings")]
    public float toggleFrequency = 1f;         // ������� ������������ (� ��������)

    private bool isPolygonColliderEnabled = true;  // ��������� PolygonCollider2D

    void Start() {
        // ���������, ��� PolygonCollider2D ��������
        if (polygonCollider == null) {
            polygonCollider = GetComponent<PolygonCollider2D>();
        }

        // ������ �������� ��� �������������� ������������
        InvokeRepeating("TogglePolygonCollider", 0f, toggleFrequency);
    }

    void TogglePolygonCollider() {
        // ������������ ��������� ��������� ����������
        isPolygonColliderEnabled = !isPolygonColliderEnabled;
        polygonCollider.enabled = isPolygonColliderEnabled;
    }
}
