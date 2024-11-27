using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerCollision : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("EdgeColliderDoor")) {
            SceneManager.LoadScene("Level3_chase");
        }
    }
}
