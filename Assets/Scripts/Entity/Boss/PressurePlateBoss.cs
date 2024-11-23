using UnityEngine;
using UnityEditor;
using System.Collections;

public class PressurePlateBoss : MonoBehaviour
{
    private bool _isActivated = false;

    private MusicPlayer musicPlayer;
    private AudioSource audioSource;
    [SerializeField] private AudioClip bossMusic; 

    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] Door closingDoor = null;
    [SerializeField] private Vector3 checkpointPosition;
    private BossAI bossAI;

    private void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        bossAI = FindObjectOfType<BossAI>();

        string path = "Assets/Music/compress.mp3"; 
        bossMusic = AssetDatabase.LoadAssetAtPath<AudioClip>(path);

        if (musicPlayer == null)
        {
            Debug.LogError("MusicPlayer не найден в сцене!");
            enabled = false;
            return;
        }
        if (bossMusic == null)
        {
            Debug.LogError("Boss music not assigned in inspector!");
            enabled = false;
            return;
        }
        musicPlayer.LoadBossMusic(bossMusic);
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying) 
        {
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && !_isActivated)
        {
            _isActivated = true;
            closingDoor.UnlockPowerToZero();
            closingDoor.CloseDoor();

            if (musicPlayer != null && bossMusic != null) 
            {
                musicPlayer.PlayBossMusic(volume);
            }

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

        PlayerPrefs.Save();
    }
}

