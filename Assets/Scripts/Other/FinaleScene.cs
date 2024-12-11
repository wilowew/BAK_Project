using UnityEngine.SceneManagement;
using UnityEngine;

public class FinaleScene : MonoBehaviour
{
    public CoinCounter coinCounter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (coinCounter != null)
            {
                coinCounter.HandleCheckpointReached();
            }
        }
    }
}
