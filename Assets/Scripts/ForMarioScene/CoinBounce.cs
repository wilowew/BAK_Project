using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBounce : MonoBehaviour
{
    public float liftHeight = 2.1f;
    public string coinTag = "coin";
    public float bounceDuration = 1f;

    public GameObject coin;

    private bool hasBounced = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasBounced)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    hasBounced = true;
                    StartCoroutine(BounceCoin());
                    break;
                }
            }
        }
    }

    private IEnumerator BounceCoin()
    {
        Vector3 startPosition = coin.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * liftHeight;

        float elapsedTime = 0f;

        while (elapsedTime < bounceDuration)
        {
            coin.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / bounceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        coin.transform.position = targetPosition;

        CoinMovement coinMovement = coin.GetComponent<CoinMovement>();
        if (coinMovement != null)
        {
            coinMovement.EnableMovement();
        }
    }
}