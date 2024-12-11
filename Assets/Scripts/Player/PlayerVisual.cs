using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string IS_RUNNING = "IsRunning";
    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_DIE = "IsDie";
    private const string TAKEHIT = "TakeHit";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        Player.Instance.OnPlayerHurt += Player_OnPlayerHurt;
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        animator.SetBool(IS_ATTACKING, Player.Instance.IsAttacking());
        if (Player.Instance.IsAlive())
        {
            AdjustPlayerFacingDirection();
        }
        Vector3 position = transform.position;
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
    }


    private void Player_OnPlayerHurt(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TAKEHIT);
    }

    private void AdjustPlayerFacingDirection()
    {
        if (PauseMenu._isPaused) return;

        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
