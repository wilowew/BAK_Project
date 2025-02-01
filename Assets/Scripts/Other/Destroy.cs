using UnityEngine;
using System.Collections;

public class Destroy: MonoBehaviour
{
    public float delay = 5f;

    void Start()
    {
        StartCoroutine(DestroyObjectCoroutine());
    }

    IEnumerator DestroyObjectCoroutine()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}