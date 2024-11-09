using UnityEngine;

public class PressurePlateBoss : MonoBehaviour
{
    private bool _isActivated = false;
    public AudioClip activationSound; 
    private AudioSource audioSource; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && !_isActivated)
        {
            _isActivated = true;

            if (activationSound != null)
            {
                audioSource.PlayOneShot(activationSound);
            }

            BossAI bossAI = FindObjectOfType<BossAI>();
            if (bossAI != null)
            {
                bossAI.OnPlayerStepOnPlate();
            }
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

