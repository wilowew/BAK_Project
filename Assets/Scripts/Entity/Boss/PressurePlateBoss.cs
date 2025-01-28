using UnityEngine;
using UnityEditor;
using System.Collections;

public class PressurePlateBoss : MonoBehaviour
{
    private bool _isActivated = false;

    [SerializeField] Door closingDoor = null;
    [SerializeField] private Vector3 checkpointPosition;
    private BossAI bossAI;

    private void Start()
    {
        bossAI = FindObjectOfType<BossAI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && !_isActivated)
        {
            _isActivated = true;
            closingDoor.UnlockPowerToZero();
            closingDoor.CloseDoor();

            if (bossAI != null)
            {
                bossAI.OnPlayerStepOnPlate();
            }

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

        CoinCounter coinCounter = FindObjectOfType<CoinCounter>();
        if (coinCounter != null)
        {
            coinCounter.SaveCoinsToCheckpoint();
        }

        PlayerPrefs.Save();
    }
}

