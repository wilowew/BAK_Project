using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    private static string SavePath => Application.persistentDataPath + "/save.json";
    private Dictionary<string, object> gameState = new Dictionary<string, object>();

    public void SaveGame(PlayerData playerData, SceneData sceneData)
    {
        Debug.Log("Метод вызвался");
        gameState["Player"] = playerData;
        gameState["Scene"] = sceneData;
        SaveFile();
    }

    public SaveFile LoadGame()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            SaveFile saveFile = JsonUtility.FromJson<SaveFile>(json);
            return saveFile;
        }
        Debug.Log("LoadedGame");
        return null;
    }

    private void SaveFile()
    {
        SaveFile saveFile = new SaveFile
        {
            playerData = (PlayerData)gameState["Player"]
        };
        string json = JsonUtility.ToJson(saveFile);
        File.WriteAllText(SavePath, json);
        Debug.Log("SaveManager saved file");
    }
}

[System.Serializable]
public class SaveFile
{
    public PlayerData playerData;
    public SceneData sceneData;
}