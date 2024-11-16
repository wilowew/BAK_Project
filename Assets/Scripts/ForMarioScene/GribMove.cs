using UnityEngine;

public class GribMovement : MonoBehaviour 
{
    public float _xSpeed = 1f; 
    private Vector3 _direction = Vector3.right; 
    private bool _isMoving = false; 

    private Rigidbody2D _rb;
    private BoxCollider2D _collider;

    private void Start() 
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        if (_rb != null) 
        {
            _rb.isKinematic = true; 
        }
        if (_collider != null)
        {
            _collider.enabled = false; 
        }
    }

    private void Update() 
    {
        if (_isMoving) 
        {
            transform.position += _direction * _xSpeed * Time.deltaTime;
        }
    }

    public void ChangeDirection()
    {
        _direction = -_direction;
    }

    public void EnableMovement()
    {
        _isMoving = true;

        if (_rb != null)
        {
            _rb.isKinematic = false;
        }
        if (_collider != null)
        {
            _collider.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Wall")) 
        {
            ChangeDirection(); 
        }
    }
}
