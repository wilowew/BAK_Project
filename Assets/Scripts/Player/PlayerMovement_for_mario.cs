using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving_for_mario : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Vector3 groundCheck;
    [SerializeField] private LayerMask groundMask;

    private Vector3 input;
    private bool IsRunning;
    private bool isGraunded;

    private Rigidbody2D rigidbody;
    private Animation_for_mario animations;
    [SerializeField] private SpriteRenderer characterSprite;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animations = GetComponentInChildren<Animation_for_mario>();
        }

    private void Update()
    {
        Move();
        CheckGround();
        if (Input.GetKeyDown(KeyCode.W) && isGraunded) {
                Jump();               
            }
    }

    private void CheckGround() {
        float radius = 0.2f;
        Vector3 rayStartPosition = transform.position + groundCheck;

        isGraunded = Physics2D.OverlapCircle(rayStartPosition, radius, groundMask) != null;
        }

    private void Move() {
        input = new Vector2(Input.GetAxis("Horizontal"), 0);
        transform.position += input * speed * Time.deltaTime;
        IsRunning = input.x != 0 ? true : false;
        if (input.x != 0) {
            characterSprite.flipX = input.x > 0 ? false : true;
            }
        animations.IsRunning = IsRunning;

        }
    private void Jump() {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
     
}
