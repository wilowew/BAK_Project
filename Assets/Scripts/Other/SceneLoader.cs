using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("LastScene"))
        {
            string lastScene = PlayerPrefs.GetString("LastScene");
            Debug.Log("��������� ��������� �����: " + lastScene);
            SceneManager.LoadScene(lastScene);
        }
        else
        {
            Debug.Log("���������� �� �������. ��������� ��������� �����.");
            SceneManager.LoadScene("Menu"); 
        }
    }
}
