using System.Collections;
using UnityEngine;

public class BossHealthRegeneration : MonoBehaviour {
    [SerializeField] private int regenerationAmount = 5; // Сколько здоровья восстанавливается за раз
    [SerializeField] private float regenerationInterval = 2f; // Интервал между восстановлением здоровья

    private EnemyEntity bossEntity; // Ссылка на сущность босса

    private void Start() {
        // Получаем компонент EnemyEntity на этом объекте
        bossEntity = GetComponent<EnemyEntity>();

        // Если босс найден, запускаем корутину восстановления здоровья
        if (bossEntity != null) {
            StartCoroutine(RegenerateHealth());
        }
        else {
            Debug.LogWarning("Boss does not have an EnemyEntity component attached!");
        }
    }

    private IEnumerator RegenerateHealth() {
        // Пока на сцене есть объекты с тегом "Gribok", продолжаем пополнять здоровье
        while (AreGribokObjectsPresent()) {
            // Проверяем, можно ли восстановить здоровье (не больше максимума)
            if (bossEntity.CurrentHealth < bossEntity.MaxHealth) {
                // Пополняем здоровье босса (передаем отрицательное значение для восстановления)
                bossEntity.TakeDamage(-regenerationAmount); // Убираем минус, чтобы восстановить здоровье
            }

            // Ждем некоторое время перед следующим пополнением здоровья
            yield return new WaitForSeconds(regenerationInterval);
        }
    }

    private bool AreGribokObjectsPresent() {
        // Ищем все объекты с тегом "Gribok" в сцене
        GameObject[] griboks = GameObject.FindGameObjectsWithTag("Gribok");

        // Если хотя бы один объект с таким тегом найден, возвращаем true
        return griboks.Length > 0;
    }
}
