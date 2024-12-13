using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint3lvl : MonoBehaviour
{
    [SerializeField] private Vector3 checkpointPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            SetCheckpoint();
        }
    }

    private void SetCheckpoint()
    {
        checkpointPosition.x -= 10f;

        DeathManager.Instance.SetCurrentCheckpoint(checkpointPosition);

        PlayerPrefs.SetFloat("CheckpointX", checkpointPosition.x);
        PlayerPrefs.SetFloat("CheckpointY", checkpointPosition.y);
        PlayerPrefs.SetFloat("CheckpointZ", checkpointPosition.z);

        PlayerPrefs.SetInt("CheckpointSet", 1);

        PlayerPrefs.Save();
    }
}


