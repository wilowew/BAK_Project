using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    public Image chargeImage; 
    public MagicAttack magicAttackScript;
    private float cooldownStartTime;
    public float maxCharges = 5f;

    void Update()
    {
        if (magicAttackScript != null && chargeImage != null)
        {
            float timeSinceLastFire = Time.time - magicAttackScript.lastFireTime;
            float cooldownDuration = magicAttackScript.fireCooldown;

            bool isCoolingDown = timeSinceLastFire < cooldownDuration;

            if (isCoolingDown)
            {
                float t = (Time.time - cooldownStartTime) / cooldownDuration;
                chargeImage.fillAmount = Mathf.Lerp(1, 0, t); 
            }
            else
            {
                float chargeLevel = (maxCharges - magicAttackScript.reload) / maxCharges;
                chargeImage.fillAmount = chargeLevel;

                cooldownStartTime = Time.time;
            }
        }
    }
}

