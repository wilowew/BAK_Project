using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving_for_mario : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Vector3 groundCheck;
    [SerializeField] private LayerMask groundMask;

    private Vector3 input;
    private bool isRunning;
    private bool isGrounded;

    private Rigidbody2D rigidbody;
    private Animation_for_mario animations;
    [SerializeField] private SpriteRenderer characterSprite;

    [SerializeField] private float scaleIncrease = 0.1f;
    [SerializeField] private float scaleDuration = 1f;

    public static bool collisionBool = false;

    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animations = GetComponentInChildren<Animation_for_mario>();
    }

    private void Update() {
        Move();
        CheckGround();
        if (Input.GetKeyDown(KeyCode.W) && isGrounded) {
            Jump();
        }
    }

    private void CheckGround() {
        Vector2 rayStartPosition = (Vector2)(transform.position + new Vector3(0, -0.15f, 0));
        RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, Vector2.down, 0.2f, groundMask);
        isGrounded = hit.collider != null;
    }

    private void Move() {
        input = new Vector2(Input.GetAxis("Horizontal"), 0);
        transform.position += input * speed * Time.deltaTime;
        isRunning = input.x != 0;

        if (input.x != 0) {
            characterSprite.flipX = input.x < 0;
        }
        animations.IsRunning = isRunning;
    }

    private void Jump() {
        rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("grib1") || collision.gameObject.CompareTag("grib2") || collision.gameObject.CompareTag("grib3")) {
            Debug.Log("Столкновение с грибом!");
            if (!collisionBool) StartCoroutine(ScalePlayer());
            Destroy(collision.gameObject);
            collisionBool = true;
        }
    }

    private IEnumerator ScalePlayer() {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale + new Vector3(scaleIncrease, scaleIncrease, 0);
        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration) {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
