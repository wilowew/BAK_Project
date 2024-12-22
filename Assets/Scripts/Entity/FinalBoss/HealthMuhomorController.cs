using UnityEngine;
using UnityEngine.UI;

public class FinalBossHealthBarController : MonoBehaviour {
    [SerializeField] private Image healthBarImage;  // Ссылка на изображение полосы здоровья
    private EnemyEntity finalBossEntity;  // Ссылка на компонент EnemyEntity для здоровья финального босса

    private bool isFinalBossPresent = false; // Флаг для отслеживания существования босса

    private void Update() {
        // Проверяем, есть ли объект с тегом "Muhomor" на сцене
        bool isBossPresent = GameObject.FindGameObjectsWithTag("Muhomor").Length > 0;

        // Если босс появился на сцене
        if (isBossPresent && !isFinalBossPresent) {
            // Находим босс-объект на сцене
            finalBossEntity = FindObjectOfType<EnemyEntity>();  // Найдем EnemyEntity на сцене
            if (finalBossEntity != null) {
                isFinalBossPresent = true;  // Устанавливаем флаг, что босс существует
                // Подписываемся на события
                finalBossEntity.OnTakeHit += UpdateFinalBossHealthBar;
                finalBossEntity.OnDeath += HandleBossDeath;  // Подписываемся на событие смерти босса
                UpdateFinalBossHealthBar(this, System.EventArgs.Empty); // Обновляем полосу здоровья сразу
            }
        }
        // Если босса больше нет на сцене
        else if (!isBossPresent && isFinalBossPresent) {
            // Убираем полосу здоровья
            SetHealthBarVisibility(false);
            isFinalBossPresent = false; // Обновляем флаг
        }
    }

    private void UpdateFinalBossHealthBar(object sender, System.EventArgs e) {
        if (finalBossEntity != null && healthBarImage != null) {
            // Рассчитываем процент здоровья
            float healthPercentage = (float)finalBossEntity.CurrentHealth / finalBossEntity.MaxHealth;

            // Устанавливаем fillAmount полосы здоровья
            healthBarImage.fillAmount = healthPercentage;
        }
    }

    private void SetHealthBarVisibility(bool isVisible) {
        // Устанавливаем видимость полосы здоровья
        gameObject.SetActive(isVisible);
    }

    private void HandleBossDeath(object sender, System.EventArgs e) {
        // Когда босс умирает, скрываем полосу здоровья
        SetHealthBarVisibility(false);
    }

    private void OnDestroy() {
        // Отписываемся от событий, если босс уничтожен
        if (finalBossEntity != null) {
            finalBossEntity.OnTakeHit -= UpdateFinalBossHealthBar;
            finalBossEntity.OnDeath -= HandleBossDeath;
        }
    }
}
