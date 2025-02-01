using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("LastScene"))
        {
            string lastScene = PlayerPrefs.GetString("LastScene");
            Debug.Log("Загружаем последнюю сцену: " + lastScene);
            SceneManager.LoadScene(lastScene);
        }
        else
        {
            Debug.Log("Сохранений не найдено. Загружаем начальную сцену.");
            SceneManager.LoadScene("Menu"); 
        }
    }
}
