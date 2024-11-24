using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    public float jumpHeight = 2f;
    public float jumpDuration = 0f;
    public float disappearDelay = 1f;

    private Vector3 startPosition;
    private bool isJumping = false;
    private bool hasDisappeared = false;

    void Start()
    {
        startPosition = transform.position;
    }

    public void EnableMovement()
    {
        if (!isJumping && !hasDisappeared)
        {
            isJumping = true;
            StartCoroutine(JumpAndDisappear());
        }
    }

    private IEnumerator JumpAndDisappear()
    {
        float elapsedTime = 0f;
        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float y = jumpHeight * 4 * t * (1 - t); 
            transform.position = startPosition + Vector3.up * y;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;

        yield return new WaitForSeconds(disappearDelay);

        if (!hasDisappeared)
        {
            hasDisappeared = true;
            Destroy(gameObject);
        }
    }
}

