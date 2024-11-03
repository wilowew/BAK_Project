using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour {

    public float speed;
    public float activationDistance = 5f;
    private Vector3 direction = Vector3.left;

    private Animator animator;
    private const string IS_DIE = "IsDie";
    private Transform player;
    private bool isMoving = false;
    private bool isInvincible = false; // Флаг бессмертия игрока
    private bool enemyInvincible = false; // Флаг бессмертия врагаx

    private void Awake() {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FixedUpdate() {
        if (Vector3.Distance(transform.position, player.position) < activationDistance) {
            isMoving = true;
        }

        if (isMoving) {
            MoveEnemy();
        }
        Debug.Log(PlayerMoving_for_mario.collisionBool);
    }

    private void MoveEnemy() {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            HandlePlayerCollision();
        }
        else if (collision.gameObject.CompareTag("Wall")) {
            ChangeDirection();
        }
    }


    private void HandlePlayerCollision() {
        if (!PlayerMoving_for_mario.collisionBool) {
            if (!isInvincible && !enemyInvincible) {
                ShrinkPlayer();
                SceneManager.LoadScene(5); 
            }
        }
        else {
            player.localScale = new Vector3(3.5f, 3.5f, 1f); // Возвращаем к стандартному масштабу
            PlayerMoving_for_mario.collisionBool = false;
            ShrinkPlayer();
        }
    }

    private void ShrinkPlayer() {
        isInvincible = true; // Устанавливаем флаг бессмертия
        enemyInvincible = true; // Устанавливаем флаг бессмертия врага
        StartCoroutine(ResetInvincibility()); // Запускаем корутину для сброса флага
    }

    private IEnumerator ResetInvincibility() {
        yield return new WaitForSeconds(0.5f); // Ждем 1 секунду
        isInvincible = false; // Сбрасываем флаг бессмертия игрока
        enemyInvincible = false; // Сбрасываем флаг бессмертия врага
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && !enemyInvincible) {
            animator.SetBool(IS_DIE, true);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(DestroyAfterDelay(0.15f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void ChangeDirection() {
        direction = -direction;
    }
}
