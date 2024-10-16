using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Добавляем ключ к инвентарю игрока
            PlayerInventory.Instance.CollectKey();

            // Убираем ключ из сцены
            gameObject.SetActive(false);
        }
    }
}
