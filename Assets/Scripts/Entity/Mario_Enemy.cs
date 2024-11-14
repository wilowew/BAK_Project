using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour 
{

    public float speed;
    public float activationDistance = 5f;
    private Vector3 direction = Vector3.left;

    private Animator animator;
    private const string IS_DIE = "IsDie";
    private Transform player;
    private bool isMoving = false;
    private bool isInvincible = false; 
    private bool enemyInvincible = false;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FixedUpdate() 
    {
        if (Vector3.Distance(transform.position, player.position) < activationDistance) 
        {
            isMoving = true;
        }

        if (isMoving) 
        {
            MoveEnemy();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !enemyInvincible)
        {
            animator.SetBool(IS_DIE, true);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(DestroyAfterDelay(0.15f));
        }
    }

    private void MoveEnemy()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            HandlePlayerCollision();
        }
        else if (collision.gameObject.CompareTag("Wall")) 
        {
            ChangeDirection();
        }
    }

    private void HandlePlayerCollision() 
    {
        if (!PlayerMoving_for_mario.collisionBool) 
        {
            if (!isInvincible && !enemyInvincible) 
            {
                ShrinkPlayer();
                SceneManager.LoadScene(5); 
            }
        }
        else 
        {
            player.localScale = new Vector3(3.5f, 3.5f, 1f); 
            PlayerMoving_for_mario.collisionBool = false;
            ShrinkPlayer();
        }
    }

    private void ShrinkPlayer() 
    {
        isInvincible = true; 
        enemyInvincible = true; 
        StartCoroutine(ResetInvincibility()); 
    }

    private IEnumerator ResetInvincibility() 
    {
        yield return new WaitForSeconds(0.5f); 
        isInvincible = false; 
        enemyInvincible = false; 
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void ChangeDirection() {
        direction = -direction;
    }
}
