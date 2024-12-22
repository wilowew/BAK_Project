using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
    public GameObject objectToSpawn;

    public Vector3 spawnPosition;
    public float objectLifetime = 40f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        Destroy(spawnedObject, objectLifetime);
    }
}
