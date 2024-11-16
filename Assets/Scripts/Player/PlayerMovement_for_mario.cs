using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving_for_mario : MonoBehaviour 
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Vector3 _groundCheck;
    [SerializeField] private LayerMask _groundMask;

    private Vector3 input;
    private bool _isRunning;
    private bool _isGrounded;

    private Rigidbody2D _rigidbody;
    private Animation_for_mario _animations;
    [SerializeField] private SpriteRenderer _characterSprite;

    [SerializeField] private float _scaleIncrease = 0.1f;
    [SerializeField] private float _scaleDuration = 1f;

    public static bool _collisionBool = false;

    private void Start() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animations = GetComponentInChildren<Animation_for_mario>();
    }

    private void Update() 
    {
        Move();
        CheckGround();
        if (Input.GetKeyDown(KeyCode.W) && _isGrounded) 
        {
            Jump();
        }
    }

    private void CheckGround() 
    {
        Vector2 _rayStartPosition = (Vector2)(transform.position + new Vector3(0, -0.15f, 0));
        RaycastHit2D hit = Physics2D.Raycast(_rayStartPosition, Vector2.down, 0.2f, _groundMask);
        _isGrounded = hit.collider != null;
    }

    private void Move() 
    {
        input = new Vector2(Input.GetAxis("Horizontal"), 0);
        transform.position += input * _speed * Time.deltaTime;
        _isRunning = input.x != 0;

        if (input.x != 0)
        {
            _characterSprite.flipX = input.x < 0;
        }
        _animations.IsRunning = _isRunning;
    }

    private void Jump() 
    {
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("grib1") || collision.gameObject.CompareTag("grib2") || collision.gameObject.CompareTag("grib3")) 
        {
            Debug.Log("Столкновение с грибом!");
            if (!_collisionBool) StartCoroutine(ScalePlayer());
            Destroy(collision.gameObject);
            _collisionBool = true;
        }
    }

    private IEnumerator ScalePlayer() 
    {
        Vector3 _originalScale = transform.localScale;
        Vector3 _targetScale = _originalScale + new Vector3(_scaleIncrease, _scaleIncrease, 0);
        float _elapsedTime = 0f;

        while (_elapsedTime < _scaleDuration) 
        {
            transform.localScale = Vector3.Lerp(_originalScale, _targetScale, _elapsedTime / _scaleDuration);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = _targetScale;
    }
}
