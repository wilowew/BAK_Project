using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerData currentPlayerData = new PlayerData();
    private SceneData currentSceneData = new SceneData { removedObjects = new List<string>() };
    private string currentCheckpoint;
    private bool currentCheckpointIsRestarting = false;

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

    public void SetCheckpoint(string checkpointId, bool isRestartingScene)
    {
        Debug.Log("Метод SetCheckpoint вызвался");
        currentCheckpoint = checkpointId;
        currentCheckpointIsRestarting = isRestartingScene;

        Player player = Player.Instance; // Получаем экземпляр игрока
        currentPlayerData = (PlayerData)player.CaptureState(); // Сохраняем состояние игрока

        var removableManager = FindObjectOfType<RemovableObjectsManager>();
        if (removableManager != null)
        {
            removableManager.SaveRemovableObjects(currentSceneData);
        }
        else
        {
            Debug.LogError("RemovableObjectsManager не найден в сцене.");
        }

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

    public void RespawnPlayer()
    {
        Debug.Log("RespawnPlayer сработал");
        if (currentCheckpointIsRestarting)
        {
            // Перезагрузка сцены с применением сохранённых данных
            RestartScene();
        }
    }

    private void RestartScene()
    {
        Debug.Log("RestartScene сработал");
        string currentSceneName = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneName);
        asyncLoad.allowSceneActivation = true; // Разрешаем активацию сцены

        asyncLoad.completed += (operation) =>
        {
            Debug.Log("Scene loaded.");
            StartCoroutine(ActivateSceneObjects());
            StartCoroutine(ApplySavedChanges());
        };
    }

    private IEnumerator ActivateSceneObjects()
    {
        Player player = Player.Instance;
        if (player != null)
        {
            player.gameObject.SetActive(true); // Убедитесь, что объект игрока активен
            player.RestoreState(currentPlayerData); // Восстанавливаем состояние игрока
        }

        // После активации можно ожидать немного, если нужно
        yield return null;
    }

    private System.Collections.IEnumerator ApplySavedChanges()
    {
        Debug.Log("ApplySavedChanges начал ждать загрузку сцены");
        yield return null; // Ждём окончания загрузки сцены

        // Применяем изменения к сцене
        var removableManager = FindObjectOfType<RemovableObjectsManager>();
        if (removableManager != null)
        {
            removableManager.LoadRemovableObjects(currentSceneData);
        }
        Debug.Log("ApplySavedChanges закончил работу");
    }

    public Vector3 GetCurrentCheckpointPosition()
    {
        // Возвращает позицию текущего чекпоинта (зависит от реализации вашей логики чекпоинтов)
        var checkpoint = FindObjectsOfType<Checkpoint>();
        foreach (var cp in checkpoint)
        {
            if (cp.checkpointId == currentCheckpoint)
            {
                return cp.transform.position;
            }
        }
        return Vector3.zero; // Возврат позиции по умолчанию
    }

    public void LoadCheckpoint()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();

        if (saveManager != null)
        {
            SaveFile save = saveManager.LoadGame();
            if (save != null)
            {
                if (currentCheckpointIsRestarting)
                {
                    RespawnPlayer();
                }
                else
                {
                    currentPlayerData = save.playerData;
                    currentSceneData = save.sceneData;
                    Debug.Log("Loaded Data");

                    // Восстановить состояние игрока
                    Player.Instance.RestoreState(currentPlayerData);
                    var removableManager = FindObjectOfType<RemovableObjectsManager>();
                    removableManager.LoadRemovableObjects(currentSceneData);
                }
            }
        }
    }

    public void SaveCurrentSceneData(RemovableObjectsManager removableManager)
    {
        removableManager.SaveRemovableObjects(currentSceneData);
        Debug.Log("Scene data saved.");
    }

    public void LoadCurrentSceneData(RemovableObjectsManager removableManager)
    {
        removableManager.LoadRemovableObjects(currentSceneData);
        Debug.Log("Scene data loaded.");
    }

    public PlayerData GetPlayerData()
    {
        return currentPlayerData;
    }
}