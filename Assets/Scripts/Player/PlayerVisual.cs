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
        Scene currentScene = SceneManager.GetActiveScene();
        if (Player.Instance.IsAlive())
        {
            AdjustPlayerFacingDirection();
        }
        Vector3 position = transform.position;
        if (position.x > 12 && position.x < 15 && position.y < -4 && position.y > -6 && Input.GetKeyDown(KeyCode.F) && currentScene.name == "Scene_1") 
        {
            SceneManager.LoadScene("Scene_2");
        }
        if (position.x > -17.5 && position.x < -15.5 && position.y < 5.4 && position.y > 4.4 && Input.GetKeyDown(KeyCode.F) && currentScene.name == "Scene_2") 
        {
            SceneManager.LoadScene("Scene_1");
        }
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
