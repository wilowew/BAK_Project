using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class BossVisual : MonoBehaviour
{
    [SerializeField] private BossAI _bossAI;
    [SerializeField] private BossEntity _bossEntity;

    private Animator _animator;

    private const string TAKEHIT = "TakeHit";
    private const string IS_DIE = "IsDie";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string SPAWN = "Spawn";

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _bossAI.OnEnemyAttack += _bossAI_OnEnemyAttack;
        _bossEntity.OnTakeHit += _bossEntity_OnTakeHit;
        _bossEntity.OnDeath += _bossEntity_OnDeath;
        _bossAI.OnEnemySpawn += _bossAI_OnEnemySpawn;
    }

    public void TriggerAttackAnimationTurnOff()
    {
        _bossEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        _bossEntity.PolygonColliderTurnOn();
    }

    private void _bossEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TAKEHIT);
    }

    private void _bossAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }

    private void _bossEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_DIE, true);
        _spriteRenderer.sortingOrder = -1;
    }

    private void _bossAI_OnEnemySpawn(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(SPAWN);
    }

    private void OnDestroy()
    {
        if (_bossAI != null)
        {
            _bossAI.OnEnemyAttack -= _bossAI_OnEnemyAttack;
            _bossAI.OnEnemySpawn -= _bossAI_OnEnemySpawn;
        }

        if (_bossEntity != null)
        {
            _bossEntity.OnTakeHit -= _bossEntity_OnTakeHit;
            _bossEntity.OnDeath -= _bossEntity_OnDeath;
        }
    }
}