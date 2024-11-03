using UnityEngine;

public class GribMovement : MonoBehaviour {
    public float xSpeed = 1f; // Скорость движения по X
    private Vector3 direction = Vector3.right; // Направление движения по X
    private bool isMoving = false; // Флаг для отслеживания состояния движения

    private Rigidbody2D rb;
    private BoxCollider2D collider;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        if (rb != null) {
            rb.isKinematic = true; // Изначально отключаем физику
        }
        if (collider != null) {
            collider.enabled = false; // Изначально отключаем коллайдер
        }
    }

    private void Update() {
        if (isMoving) {
            transform.position += direction * xSpeed * Time.deltaTime;
        }
    }

    public void EnableMovement() {
        isMoving = true; // Включаем движение

        if (rb != null) {
            rb.isKinematic = false; // Разрешаем физику
        }
        if (collider != null) {
            collider.enabled = true; // Включаем коллайдер
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Меняем направление у grib1 при столкновении с Wall
        if (collision.gameObject.CompareTag("Wall")) {
            ChangeDirection(); // Меняем направление
        }
    }

    public void ChangeDirection() {
        direction = -direction; // Меняем направление
    }
}
