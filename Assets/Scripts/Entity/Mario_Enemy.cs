using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour 
{

    public float _speed;
    public float _activationDistance = 5f;
    private Vector3 _direction = Vector3.left;

    private Animator _animator;
    private const string IS_DIE = "IsDie";
    private Transform _player;
    private bool _isMoving = false;
    private bool _isInvincible = false; 
    private bool _enemyInvincible = false;

    private void Awake() 
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FixedUpdate() 
    {
        if (Vector3.Distance(transform.position, _player.position) < _activationDistance) 
        {
            _isMoving = true;
        }

        if (_isMoving) 
        {
            MoveEnemy();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_enemyInvincible)
        {
            _animator.SetBool(IS_DIE, true);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(DestroyAfterDelay(0.15f));
        }
    }

    private void MoveEnemy()
    {
        transform.position += _direction * _speed * Time.fixedDeltaTime;
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
        if (!PlayerMoving_for_mario._collisionBool) 
        {
            if (!_isInvincible && !_enemyInvincible) 
            {
                ShrinkPlayer();
                SceneManager.LoadScene(9); 
            }
        }
        else 
        {
            _player.localScale = new Vector3(3.5f, 3.5f, 1f); 
            PlayerMoving_for_mario._collisionBool = false;
            ShrinkPlayer();
        }
    }

    private void ShrinkPlayer() 
    {
        _isInvincible = true; 
        _enemyInvincible = true; 
        StartCoroutine(ResetInvincibility()); 
    }

    private IEnumerator ResetInvincibility() 
    {
        yield return new WaitForSeconds(0.5f); 
        _isInvincible = false; 
        _enemyInvincible = false; 
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void ChangeDirection() 
    {
        _direction = -_direction;
    }
}
