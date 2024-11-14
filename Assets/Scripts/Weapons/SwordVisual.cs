using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SwordVisual : MonoBehaviour
{
    [SerializeField] private Sword _sword;

    private Animator _animator;
    private const string ATTACK = "Attack";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _sword.OnSwordSwing += Sword_OnSwordSwing;
    }

    public void TriggerEndAttackAnimation()
    {
        _sword.AttackColliderTurnOff();
    }

    private void Sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
}
