using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyColorChange : MonoBehaviour {
    [Header("Enemy Settings")]
    [SerializeField] private EnemyEntity enemyEntity;  // Ссылка на компонент EnemyEntity
    [SerializeField] private float cycleSpeed = 1f;  // Частота изменения канала S (по оси времени)
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyEntity == null) {
            enemyEntity = GetComponent<EnemyEntity>();  // Если не привязан вручную, то попытаемся найти автоматически
        }
    }

    private void Update() {
        if (enemyEntity != null) {
            UpdateEnemyColor();
        }
    }

    private void UpdateEnemyColor() {
        // Рассчитываем процент здоровья
        float healthPercentage = (float)enemyEntity.CurrentHealth / enemyEntity.MaxHealth;
        // Применяем линейное изменение яркости от 0.3 (минимум) до 1 (максимум)
        float brightness = Mathf.Lerp(0.3f, 1f, healthPercentage);

        // Преобразуем текущий цвет в HSV
        Color currentColor = spriteRenderer.color;
        Color.RGBToHSV(currentColor, out float h, out float s, out float v);

        // Циклически изменяем S от 0 до 0.5
        s = Mathf.PingPong(Time.time * cycleSpeed, 0.5f);  // От 0 до 0.5 с частотой, зависящей от cycleSpeed

        // Мы оставляем H как было, чтобы сохранить исходный оттенок.
        // Применяем новый цвет с измененным S и яркостью (V)
        Color newColor = Color.HSVToRGB(h, s, brightness);

        // Применяем новый цвет
        spriteRenderer.color = newColor;
    }
}
