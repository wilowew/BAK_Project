using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject _pauseMenu;
    public static bool _isPaused;

    private void Start()
    {
        _pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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

    public void PauseGame()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("ResumeGame called");
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
