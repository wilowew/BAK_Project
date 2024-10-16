using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    [SerializeField] private Door door; // Ссылка на объект двери

    private void Update()
    {
        // Проверяем, открыта ли дверь
        if (!door.IsLocked()) // Если дверь разблокирована
        {
            Collider2D[] doorColliders = door.GetComponents<Collider2D>();
            bool allCollidersDisabled = true;

            // Проверяем, отключены ли все коллайдеры двери
            foreach (Collider2D col in doorColliders)
            {
                if (col.enabled)
                {
                    allCollidersDisabled = false;
                    break;
                }
            }

            // Если все коллайдеры двери отключены, удаляем чёрный экран
            if (allCollidersDisabled)
            {
                Destroy(gameObject); // Удаляем объект с чёрным экраном
                Debug.Log("Чёрный экран удалён после открытия двери");
            }
        }
    }
}