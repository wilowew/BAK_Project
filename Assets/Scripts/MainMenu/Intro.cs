using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public int waitTime;
    [SerializeField] public int _numberScene;

    void Start()
    {
        StartCoroutine(WaitForLevel());
    }

    IEnumerator WaitForLevel()
    {
        yield return new WaitForSeconds (waitTime);
        SceneManager.LoadScene(_numberScene);
    }
}
  
