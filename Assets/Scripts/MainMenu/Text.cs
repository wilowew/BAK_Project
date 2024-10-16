using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    private Text blinkingText;
    public float speed = 1f; // Скорость мигания

    private void Start()
    {
        blinkingText = GetComponent<Text>();
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        Color textColor = blinkingText.color;
        float alpha = 0.8f;

        while (true)
        {
            // Плавно увеличиваем прозрачность до 0.8 (80%)
            while (alpha < 0.8f)
            {
                alpha += Time.deltaTime * speed;
                SetTextAlpha(alpha);
                yield return null;
            }

            // Плавно уменьшаем прозрачность до 0
            while (alpha > 0.3f)
            {
                alpha -= Time.deltaTime * speed;
                SetTextAlpha(alpha);
                yield return null;
            }
        }
    }

    // Установка альфа-канала для текста
    private void SetTextAlpha(float alpha)
    {
        Color color = blinkingText.color;
        color.a = Mathf.Clamp(alpha, 0f, 0.8f); // Ограничиваем альфа до 80% (0.8)
        blinkingText.color = color;
    }
}