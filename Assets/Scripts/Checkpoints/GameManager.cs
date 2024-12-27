using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerData currentPlayerData = new PlayerData();
    private SceneData currentSceneData = new SceneData { collectedItems = new List<string>(), defeatedEnemies = new List<string>() };
    private string currentCheckpoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(string checkpointId)
    {
        Debug.Log("Метод SetCheckpoint вызвался");
        currentCheckpoint = checkpointId;

        Player player = Player.Instance; // Получаем экземпляр игрока
        currentPlayerData = (PlayerData)player.CaptureState(); // Сохраняем состояние игрока

        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.SaveGame(currentPlayerData, currentSceneData);
            Debug.Log("Saved Data");
        }
        else
        {
            Debug.LogError("SaveManager не найден! Убедитесь, что он присутствует на сцене.");
        }
    }

    public void LoadCheckpoint()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();

        if (saveManager != null)
        {
            SaveFile save = saveManager.LoadGame();
            if (save != null)
            {
                currentPlayerData = save.playerData;
                currentSceneData = save.sceneData;
                Debug.Log("Loaded Data");

                // Восстановить состояние игрока
                Player.Instance.RestoreState(currentPlayerData);
            }
        }
    }

    public PlayerData GetPlayerData()
    {
        return currentPlayerData;
    }
}