using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [Header("Music Players")]
    [SerializeField] private MusicPlayer activeMusicPlayer; // ������� �������� MusicPlayer
    [SerializeField] private MusicPlayer newMusicPlayer; // ����� MusicPlayer, �� ������� �������������

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        Debug.Log("Player entered music trigger.");

        // ��������� ������� MusicPlayer
        if (activeMusicPlayer != null)
        {
            Debug.Log($"Disabling current MusicPlayer: {activeMusicPlayer.name}");
            activeMusicPlayer.PauseMusic();
            activeMusicPlayer.enabled = false; // ��������� ���������
            activeMusicPlayer.gameObject.SetActive(false); // ��������� ������
        }

        // �������� ����� MusicPlayer
        if (newMusicPlayer != null)
        {
            newMusicPlayer.gameObject.SetActive(true); // �������� ������ ������ MusicPlayer
            newMusicPlayer.enabled = true; // �������� ��������� MusicPlayer
            Debug.Log($"Enabling new MusicPlayer: {newMusicPlayer.name}");
            newMusicPlayer.Initialize(); // �������������� MusicPlayer
            newMusicPlayer.ResumeMusic(); // ������������ ���������������
        }
    }
}