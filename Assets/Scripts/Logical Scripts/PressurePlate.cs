using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Sprite pressedSprite;  // Спрайт для нажатого состояния
    private SpriteRenderer spriteRenderer;
    private bool isPressed = false;
    [SerializeField] private Door correspondingDoor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            spriteRenderer.sprite = pressedSprite;
            correspondingDoor.UnlockDoorByPlate();  // Отправляем сигнал двери об открытии
        }
    }
}