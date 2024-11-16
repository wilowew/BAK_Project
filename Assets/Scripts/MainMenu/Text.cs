using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    private Text blinkingText;
    public float speed = 1f; 

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
            while (alpha < 0.8f)
            {
                alpha += Time.deltaTime * speed;
                SetTextAlpha(alpha);
                yield return null;
            }

            while (alpha > 0.3f)
            {
                alpha -= Time.deltaTime * speed;
                SetTextAlpha(alpha);
                yield return null;
            }
        }
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = blinkingText.color;
        color.a = Mathf.Clamp(alpha, 0f, 0.8f);
        blinkingText.color = color;
    }
}