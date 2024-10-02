using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string IS_RUNNING = "IsRunning";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        AdjustPlayerFacingDirection();
        Scene currentScene = SceneManager.GetActiveScene();
        Vector3 position = transform.position;
        if (position.x > 12 && position.x < 15 && position.y < -4 && position.y > -6 && Input.GetKeyDown(KeyCode.F) && currentScene.name == "Scene_1") {
            SceneManager.LoadScene("Scene_2");
            }
        if (position.x > -8 && position.x < -6 && position.y < 1 && position.y > 0 && Input.GetKeyDown(KeyCode.F) && currentScene.name == "Scene_2") {
            SceneManager.LoadScene("Scene_1");
            }
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
