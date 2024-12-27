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
        Debug.Log("����� SetCheckpoint ��������");
        currentCheckpoint = checkpointId;

        Player player = Player.Instance; // �������� ��������� ������
        currentPlayerData = (PlayerData)player.CaptureState(); // ��������� ��������� ������

        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.SaveGame(currentPlayerData, currentSceneData);
            Debug.Log("Saved Data");
        }
        else
        {
            Debug.LogError("SaveManager �� ������! ���������, ��� �� ������������ �� �����.");
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

                // ������������ ��������� ������
                Player.Instance.RestoreState(currentPlayerData);
            }
        }
    }

    public PlayerData GetPlayerData()
    {
        return currentPlayerData;
    }
}