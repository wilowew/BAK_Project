using UnityEngine;

public class PressurePlateBoss : MonoBehaviour
{
    private bool _isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && !_isActivated)
        {
            _isActivated = true;
            BossAI bossAI = FindObjectOfType<BossAI>();
            if (bossAI != null)
            {
                bossAI.OnPlayerStepOnPlate();
            }
        }
    }
}

