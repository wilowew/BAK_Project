using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{
    private Vector2 originalPosition;
    private bool isBouncing;

    private void Start() 
    {
        originalPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player") && !isBouncing) 
        {
            foreach (ContactPoint2D contact in collision.contacts) 
            {
                if (contact.normal.y > 0.5f) 
                {
                    isBouncing = true;
                    StartCoroutine(Bounce());
                    break;
                }
            }
        }
    }

    private IEnumerator Bounce() 
    {
        Vector2 bouncePosition = (Vector2)transform.position + new Vector2(0, 0.5f);
        float elapsedTime = 0f;
        float bounceDuration = 0.2f;

        while (elapsedTime < bounceDuration) 
        {
            transform.position = Vector2.Lerp(transform.position, bouncePosition, elapsedTime / bounceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < bounceDuration) 
        {
            transform.position = Vector2.Lerp(transform.position, originalPosition, elapsedTime / bounceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isBouncing = false;
    }
}
