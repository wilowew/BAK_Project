using UnityEngine;

public class PressurePlateBoss : MonoBehaviour
{
    private bool _isActivated = false;
    public AudioClip bossMusic;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;
    private AudioSource audioSource;
    private MusicPlayer musicPlayer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ������������� ��������� ��� ������ �����
        audioSource.volume = volume;

        // ����� ��������� MusicPlayer �� �����
        musicPlayer = FindObjectOfType<MusicPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && !_isActivated)
        {
            _isActivated = true;

            // ��������� ������� ������
            if (musicPlayer != null)
            {
                musicPlayer.PauseMusic();
            }

            // ������������� ������ ��� �����
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
        }
    }

    private void Update()
    {
        // ���������, ����������� �� ������ �����, ����� ������� �������
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
}