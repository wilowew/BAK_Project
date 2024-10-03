using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public int waitTime;

    void Start()
    {
        StartCoroutine(WaitForLevel());
    }

    IEnumerator WaitForLevel()
    {
        yield return new WaitForSeconds (waitTime);
        SceneManager.LoadScene(2);
    }
}
  
