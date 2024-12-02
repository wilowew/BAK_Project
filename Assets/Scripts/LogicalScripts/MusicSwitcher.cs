using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [Header("Music Players")]
    [SerializeField] private MusicPlayer activeMusicPlayer;
    [SerializeField] private MusicPlayer newMusicPlayer;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        Debug.Log("Player entered music trigger.");

        if (activeMusicPlayer != null)
        {
            Debug.Log($"Disabling current MusicPlayer: {activeMusicPlayer.name}");
            activeMusicPlayer.PauseMusic();
        }

        if (newMusicPlayer != null)
        {
            Debug.Log($"Enabling new MusicPlayer: {newMusicPlayer.name}");
            newMusicPlayer.Initialize(); // Убедимся, что AudioSource настроен
            newMusicPlayer.ResumeMusic();
        }
    }
}