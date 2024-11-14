using UnityEngine;

public class GribMovement : MonoBehaviour 
{
    public float xSpeed = 1f; 
    private Vector3 direction = Vector3.right; 
    private bool isMoving = false; 

    private Rigidbody2D rb;
    private BoxCollider2D collider;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        if (rb != null) 
        {
            rb.isKinematic = true; 
        }
        if (collider != null)
        {
            collider.enabled = false; 
        }
    }

    private void Update() 
    {
        if (isMoving) 
        {
            transform.position += direction * xSpeed * Time.deltaTime;
        }
    }

    public void EnableMovement()
    {
        isMoving = true;

        if (rb != null)
        {
            rb.isKinematic = false;
        }
        if (collider != null)
        {
            collider.enabled = true;
        }
    }

    public void ChangeDirection()
    {
        direction = -direction;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Wall")) 
        {
            ChangeDirection(); 
        }
    }
}
