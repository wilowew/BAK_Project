using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour {
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            LoadScene();
        }
    }

    private void LoadScene() {
        if (!string.IsNullOrEmpty(sceneToLoad)) {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
