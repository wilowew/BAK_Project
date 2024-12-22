using UnityEngine;

public class EnemyColliderToggle : MonoBehaviour {
    [Header("Colliders")]
    public PolygonCollider2D polygonCollider;  // Ссылка на PolygonCollider2D

    [Header("Settings")]
    public float toggleFrequency = 1f;         // Частота переключения (в секундах)

    private bool isPolygonColliderEnabled = true;  // Состояние PolygonCollider2D

    void Start() {
        // Убедитесь, что PolygonCollider2D привязан
        if (polygonCollider == null) {
            polygonCollider = GetComponent<PolygonCollider2D>();
        }

        // Запуск корутины для периодического переключения
        InvokeRepeating("TogglePolygonCollider", 0f, toggleFrequency);
    }

    void TogglePolygonCollider() {
        // Переключение состояния активного коллайдера
        isPolygonColliderEnabled = !isPolygonColliderEnabled;
        polygonCollider.enabled = isPolygonColliderEnabled;
    }
}
