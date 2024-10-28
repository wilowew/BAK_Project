//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Enemy : MonoBehaviour {

//    public float speed;
//    public Vector3[] positions;

//    private int currentTarget;
//    private Animator animator;
//    private const string IS_DIE = "IsDie";

//    private void Awake() {
//        animator = GetComponent<Animator>();
//    }

//    public void FixedUpdate() {
//        transform.position = Vector3.MoveTowards(transform.position, positions[currentTarget], speed);
//        if (transform.position == positions[currentTarget]) {
//            if (currentTarget < positions.Length - 1) {
//                currentTarget++;
//            }
//            else {
//                currentTarget = 0;
//            }
//        }
//    }

//    private void OnCollisionEnter2D(Collision2D collision) {
//        if (collision.gameObject.tag == "Player") {
//            SceneManager.LoadScene(5);
//        }
//    }

//    public void OnTriggerEnter2D(Collider2D collision) {
//        if (collision.gameObject.tag == "Player") {
//            animator.SetBool(IS_DIE, true);
//            GetComponent<Collider2D>().enabled = false;
//            StartCoroutine(DestroyAfterDelay(0.15f));
//        }
//    }

//    private IEnumerator DestroyAfterDelay(float delay) {
//        yield return new WaitForSeconds(delay);
//        Destroy(gameObject);
//    }
//} 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour {

    public float speed;
    public float activationDistance = 5f; // ����������, ��� ������� ���� �������� ��������
    private Vector3 direction = Vector3.left; // �������� ������ �����

    private Animator animator;
    private const string IS_DIE = "IsDie";
    private Transform player; // ������ �� ������
    private bool isMoving = false; // ����, �����������, �������� �� ����

    private void Awake() {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // ������� ������ �� ����
    }

    public void FixedUpdate() {
        // ��������� ���������� �� ������
        if (Vector3.Distance(transform.position, player.position) < activationDistance) {
            isMoving = true;
        }

        if (isMoving) {
            MoveEnemy();
        }
    }

    private void MoveEnemy() {
        // ��������� � ������� �����������
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene(5);
        }
        else if (collision.gameObject.CompareTag("Wall")) {
            ChangeDirection(); // ������ ����������� ��� ������������ �� ������
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
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
        direction = -direction; // ������ �����������
    }
}
