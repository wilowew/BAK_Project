using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject sword;

    private bool isSwordActive = false;

    private void Start()
    {
        sword.SetActive(false); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isSwordActive = !isSwordActive;
            sword.SetActive(isSwordActive);
        }
    }
}
