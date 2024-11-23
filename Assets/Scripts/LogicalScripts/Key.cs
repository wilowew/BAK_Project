using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private Door correspondingDoor; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision is BoxCollider2D)
        {
            correspondingDoor.increaseUnlockPower(); 
            Debug.Log("Key collected");
            Destroy(gameObject);
        }
    }
}