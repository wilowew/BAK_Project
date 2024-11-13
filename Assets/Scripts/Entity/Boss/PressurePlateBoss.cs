using UnityEngine;

public class PressurePlateBoss : MonoBehaviour
{
    private bool _isActivated = false;
    public AudioClip bossMusic;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] Door closingDoor = null;
    private AudioSource audioSource;
    private MusicPlayer musicPlayer;
    [SerializeField] private Vector3 checkpointPosition;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.volume = volume;
        musicPlayer = FindObjectOfType<MusicPlayer>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && !_isActivated)
        {
            _isActivated = true;

            closingDoor.UnlockPowerToZero();
            closingDoor.CloseDoor();

            if (musicPlayer != null)
            {
                musicPlayer.PauseMusic();
            }

            if (bossMusic != null)
            {
                audioSource.clip = bossMusic;
                audioSource.Play();
            }

            BossAI bossAI = FindObjectOfType<BossAI>();
            if (bossAI != null)
            {
                bossAI.OnPlayerStepOnPlate();
            }

            SetCheckpoint();
        }
    }

    private void Update()
    {

        if (_isActivated && !audioSource.isPlaying && musicPlayer != null)
        {
            musicPlayer.ResumeMusic();
            _isActivated = false;
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
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