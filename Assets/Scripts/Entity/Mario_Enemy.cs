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
    public float activationDistance = 5f; // Расстояние, при котором враг начинает движение
    private Vector3 direction = Vector3.left; // Движение всегда влево

    private Animator animator;
    private const string IS_DIE = "IsDie";
    private Transform player; // Ссылка на игрока
    private bool isMoving = false; // Флаг, указывающий, движется ли враг

    private void Awake() {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Находим игрока по тегу
    }

    public void FixedUpdate() {
        // Проверяем расстояние до игрока
        if (Vector3.Distance(transform.position, player.position) < activationDistance) {
            isMoving = true;
        }

        if (isMoving) {
            MoveEnemy();
        }
    }

    private void MoveEnemy() {
        // Двигаемся в текущем направлении
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene(5);
        }
        else if (collision.gameObject.CompareTag("Wall")) {
            ChangeDirection(); // Меняем направление при столкновении со стеной
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
        direction = -direction; // Меняем направление
    }
}
