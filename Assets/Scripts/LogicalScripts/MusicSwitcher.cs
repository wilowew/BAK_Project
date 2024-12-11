using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [Header("Music Players")]
    [SerializeField] private MusicPlayer activeMusicPlayer; // Текущий активный MusicPlayer
    [SerializeField] private MusicPlayer newMusicPlayer; // Новый MusicPlayer, на который переключаемся

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        Debug.Log("Player entered music trigger.");

        // Отключаем текущий MusicPlayer
        if (activeMusicPlayer != null)
        {
            Debug.Log($"Disabling current MusicPlayer: {activeMusicPlayer.name}");
            activeMusicPlayer.PauseMusic();
            activeMusicPlayer.enabled = false; // Отключаем компонент
            activeMusicPlayer.gameObject.SetActive(false); // Отключаем объект
        }

        // Включаем новый MusicPlayer
        if (newMusicPlayer != null)
        {
            newMusicPlayer.gameObject.SetActive(true); // Включаем объект нового MusicPlayer
            newMusicPlayer.enabled = true; // Включаем компонент MusicPlayer
            Debug.Log($"Enabling new MusicPlayer: {newMusicPlayer.name}");
            newMusicPlayer.Initialize(); // Инициализируем MusicPlayer
            newMusicPlayer.ResumeMusic(); // Возобновляем воспроизведение
        }
    }
}