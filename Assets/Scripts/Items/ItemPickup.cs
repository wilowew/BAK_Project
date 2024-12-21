using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private float pickupRadius = 2f;
    [SerializeField] private SpriteRenderer itemSprite;
    [SerializeField] private ParticleSystem pickupEffect;
    [SerializeField] private string nextSceneName;

    public enum ItemType
    {
        HealthPotion,
        SpeedPotion,
        Teleport
    }

    public ItemType itemType;

    private void Update()
    {
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) <= pickupRadius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickupItem();
            }
        }
    }

    private void PickupItem()
    {
        Instantiate(pickupEffect, transform.position, Quaternion.identity).Play();

        switch (itemType)
        {
            case ItemType.HealthPotion:
                Player.Instance.Heal(10);
                break;

            case ItemType.SpeedPotion:
                Player.Instance.IncreaseSpeed(1.5f, 5f);
                break;
            case ItemType.Teleport:
                TeleportToNextScene();
                break;
        }

        Destroy(gameObject);
    }

    private void TeleportToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
