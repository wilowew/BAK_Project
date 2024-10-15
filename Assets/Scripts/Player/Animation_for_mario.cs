using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_for_mario : MonoBehaviour
{

    private Animator animator;
    public bool IsRunning { private get; set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        }

    private void FixedUpdate()
    {
        animator.SetBool("IsRunning", IsRunning);

    }
}
