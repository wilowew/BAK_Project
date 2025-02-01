
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaver : MonoBehaviour
{
    public void SaveCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetString("LastScene", currentScene);
        PlayerPrefs.Save();

        Debug.Log("Сохранили сцену: " + currentScene);
    }
}
