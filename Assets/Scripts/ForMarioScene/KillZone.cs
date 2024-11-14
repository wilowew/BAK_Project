using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour 
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Enemy")) 
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player")) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
