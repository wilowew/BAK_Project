using UnityEngine;

public class ColorCycling : MonoBehaviour {
    [Header("Color Cycling Settings")]
    [SerializeField] private float colorChangeSpeed = 1f;  // Скорость смены цвета
    private SpriteRenderer spriteRenderer;
    private bool isBossAlive = true;

    private readonly Color initialColor = Color.HSVToRGB(121f / 360f, 38f / 100f, 45f / 100f); // H = 121, S = 38, V = 45

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start() {
        // Устанавливаем начальный цвет (когда босс жив)
        spriteRenderer.color = initialColor;
    }

    void Update() {
        // Проверяем, существует ли объект с тегом "Muhomor" на сцене
        bool bossExists = GameObject.FindWithTag("Muhomor") != null;

        if (!bossExists && isBossAlive) {
            // Если босса нет на сцене, начинаем плавное изменение цвета
            isBossAlive = false;
            StartCoroutine(CycleColor());
        }
        else if (bossExists && !isBossAlive) {
            // Если босс снова на сцене, останавливаем цикл изменения цвета
            isBossAlive = true;
            StopCoroutine(CycleColor());
            spriteRenderer.color = initialColor;  // Восстанавливаем исходный цвет (когда босс жив)
        }
    }

    private System.Collections.IEnumerator CycleColor() {
        float hue = initialColor.r;  // Начальный оттенок (H) из изначального цвета

        while (!isBossAlive) {
            // Преобразуем значение оттенка в цвет (V = 1 - максимальная яркость)
            Color color = Color.HSVToRGB(hue, 1f, 1f);  // S = 1, V = 1

            // Применяем цвет к объекту
            spriteRenderer.color = color;

            // Увеличиваем оттенок (hue) для перехода к следующему цвету
            hue += Time.deltaTime * colorChangeSpeed;
            if (hue > 1f) hue = 0f; // Когда hue достигает 1 (красный), возвращаемся к 0 (также красный)

            yield return null; // Ждем до следующего кадра
        }
    }
}
