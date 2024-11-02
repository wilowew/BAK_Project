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
}
