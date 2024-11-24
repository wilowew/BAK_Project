using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 400f;
    private float currentTime;
    public Text timerText; 

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
        {
            SceneManager.LoadScene(5); 
        }
    }
}
