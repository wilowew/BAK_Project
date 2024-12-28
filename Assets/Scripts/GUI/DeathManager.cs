using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Text deathScoreText;
    [SerializeField] private GameObject magicCounter;
    [SerializeField] private GameObject swordHandler;
    [SerializeField] private HealthDisplay healthDisplay;

    private Vector3 currentCheckpointPosition;
    private Vector3 initialPlayerPosition;

    public static DeathManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        deathPanel.SetActive(false);
        Player.Instance.OnPlayerDeath += HandlePlayerDeath;

        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.DeleteKey("CheckpointSet");

        initialPlayerPosition = Player.Instance.transform.position;

        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            currentCheckpointPosition = new Vector3(x, y, z);
            Player.Instance.transform.position = currentCheckpointPosition;
        }
        else
        {
            currentCheckpointPosition = Player.Instance.transform.position;
        }
    }

    public void RestartPlayerAtCheckpoint()
    {
        CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();

        if (PlayerPrefs.GetInt("CheckpointSet", 0) == 1)
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            currentCheckpointPosition = new Vector3(x, y, z);
            Player.Instance.transform.position = currentCheckpointPosition;
        }
        else
        {
            Player.Instance.transform.position = initialPlayerPosition;
        }

        if (cameraFollow != null)
        {
            cameraFollow.SnapToTarget();
        }

    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetCurrentCheckpoint(Vector3 position)
    {
        currentCheckpointPosition = position;
    }

    private void ShowDeathPanel()
    {
        if (deathPanel != null)
        {
            Time.timeScale = 0f;
            deathPanel.SetActive(true);
        }
    }

    public bool GetDeathState()
    {
        return deathPanel != null && deathPanel.activeSelf;
    }

    private void HandlePlayerDeath(object sender, System.EventArgs e)
    {
        ShowDeathPanel();
        Time.timeScale = 0f; // ������������ ���� ������ �� ����� ����������� ������ ������.

        if (magicCounter != null)
        {
            magicCounter.SetActive(false);
        }
        if (swordHandler != null)
        {
            swordHandler.SetActive(false);
        }

        // ��������� ���������� ������
        GameManager.Instance.LoadCheckpoint();

        // ����������� ������ ������ � ����������� ���� ����� ��������� ������
        StartCoroutine(RestartGameAfterDelay());
    }

    private IEnumerator RestartPlayer()
    {
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1f;
        deathPanel.SetActive(false);
        RestartGameAfterDelay();
    }

    private IEnumerator RestartGameAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f); // ���� 5 ������ �� ������ ������
        Time.timeScale = 1f; // ������� ��������� �������
        deathPanel.SetActive(false); // ������� ������ ������

        // ��������������� ��������� � ����������
        if (magicCounter != null)
        {
            magicCounter.SetActive(true);
        }
        if (swordHandler != null)
        {
            swordHandler.SetActive(true);
        }
        if (healthDisplay != null)  // ������ �������� ����������
        {
            healthDisplay.ShowHealthDisplay();
        }
        RevivePlayer(); // ������ ������ ������, ������� ������� ����������
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player.Instance.Revive();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void RevivePlayer()
    {
        Player.Instance.Revive();
        gameObject.SetActive(true);
    }
}