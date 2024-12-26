using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject _pauseMenu;
    public static bool _isPaused;

    private MusicPlayer activeMusicPlayer;

    private void Start()
    {
        _pauseMenu.SetActive(false);
        FindActiveMusicPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindActiveMusicPlayer();
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void FindActiveMusicPlayer()
    {
        MusicPlayer[] allMusicPlayers = FindObjectsOfType<MusicPlayer>();

        activeMusicPlayer = null;

        foreach (var musicPlayer in allMusicPlayers)
        {
            if (musicPlayer.gameObject.activeInHierarchy && musicPlayer.enabled)
            {
                activeMusicPlayer = musicPlayer;
                break;
            }
        }

        if (activeMusicPlayer == null)
        {
            Debug.LogWarning("Active MusicPlayer not found. Music management will be skipped.");
        }
        else
        {
            Debug.Log($"Active MusicPlayer found: {activeMusicPlayer.name}");
        }
    }

    public void PauseGame()
    {
        if (!Player.Instance.IsAlive()) return;

        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;

        if (activeMusicPlayer != null)
        {
            activeMusicPlayer.PauseMusic();
        }
    }

    public void ResumeGame()
    {
        if (!Player.Instance.IsAlive()) return;

        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;

        if (activeMusicPlayer != null)
        {
            activeMusicPlayer.ResumeMusic();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}