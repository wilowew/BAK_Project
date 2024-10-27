using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int Value;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            Player.Instance.CollectCoins(Value);
            Destroy(gameObject);
        }
    }
}
