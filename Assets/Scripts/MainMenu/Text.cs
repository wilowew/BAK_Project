using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    private Text blinkingText;
    public float _speed = 1f; 

    private void Start()
    {
        blinkingText = GetComponent<Text>();
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        Color _textColor = blinkingText.color;
        float _alpha = 0.8f;

        while (true)
        {
            while (_alpha < 0.8f)
            {
                _alpha += Time.deltaTime * _speed;
                SetTextAlpha(_alpha);
                yield return null;
            }

            while (_alpha > 0.3f)
            {
                _alpha -= Time.deltaTime * _speed;
                SetTextAlpha(_alpha);
                yield return null;
            }
        }
    }

    private void SetTextAlpha(float _alpha)
    {
        Color color = blinkingText.color;
        color.a = Mathf.Clamp(_alpha, 0f, 0.8f);
        blinkingText.color = color;
    }
}