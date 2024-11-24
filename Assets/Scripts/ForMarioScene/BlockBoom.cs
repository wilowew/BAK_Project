using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    private Vector2 _originalPosition;
    private bool _isBouncing;

    private void Start()
    {
        _originalPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isBouncing)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    _isBouncing = true;
                    StartCoroutine(Bounce());
                    break;
                }
            }
        }
    }

    private IEnumerator Bounce()
    {
        Vector2 _bouncePosition = (Vector2)transform.position + new Vector2(0, 0.5f);
        float _elapsedTime = 0f;
        float _bounceDuration = 0.2f;

        while (_elapsedTime < _bounceDuration)
        {
            transform.position = Vector2.Lerp(transform.position, _bouncePosition, _elapsedTime / _bounceDuration);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _elapsedTime = 0f;

        while (_elapsedTime < _bounceDuration)
        {
            transform.position = Vector2.Lerp(transform.position, _originalPosition, _elapsedTime / _bounceDuration);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _originalPosition;
        _isBouncing = false;
    }
}