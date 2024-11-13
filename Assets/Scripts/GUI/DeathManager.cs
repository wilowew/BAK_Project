using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Text deathScoreText;

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

    private void ShowDeathPanel()
    {
        if (deathPanel != null)
        {
            Time.timeScale = 0f;
            deathPanel.SetActive(true);
        }
    }

    private void HandlePlayerDeath(object sender, System.EventArgs e)
    {
        ShowDeathPanel();
        int collectedCoins = FindObjectOfType<CoinCounter>().GetCoins();
        deathScoreText.text = $"Score: {collectedCoins}";
        StartCoroutine(RestartSceneAutomatically());
    }

    private IEnumerator RestartPlayer()
    {
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1f;
        deathPanel.SetActive(false);
        RestartPlayerAtCheckpoint();
    }

    private IEnumerator RestartSceneAutomatically()
    {
        yield return new WaitForSecondsRealtime(5f);
        RestartScene();
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player.Instance.Revive();
        RestartPlayerAtCheckpoint();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetCurrentCheckpoint(Vector3 position)
    {
        currentCheckpointPosition = position;
    }

    private void RevivePlayer()
    {
        Player.Instance.Revive();
        gameObject.SetActive(true);
    }
}