using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarController : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;
    [SerializeField] private float displayDistance = 10f;
    private BossEntity _bossEntity;
    private Transform playerTransform;

    private void Start()
    {
        _bossEntity = FindObjectOfType<BossEntity>();
        playerTransform = FindObjectOfType<Player>().transform;

        if (_bossEntity != null)
        {
            _bossEntity.OnTakeHit += UpdateHealthBar;
            _bossEntity.OnDeath += (sender, e) => gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_bossEntity != null && playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(_bossEntity.transform.position, playerTransform.position);

            SetHealthBarVisibility(distanceToPlayer <= displayDistance && _bossEntity.IsAlive());
        }
    }

    private void UpdateHealthBar(object sender, System.EventArgs e)
    {
        if (_bossEntity != null && healthFillImage != null)
        {
            float healthPercentage = (float)_bossEntity._currentHealth / _bossEntity._enemySO.enemyHealth;
            healthFillImage.fillAmount = healthPercentage;

            float distanceToPlayer = Vector3.Distance(_bossEntity.transform.position, playerTransform.position);
            gameObject.SetActive(distanceToPlayer <= displayDistance && _bossEntity.IsAlive());
        }
    }

    private void SetHealthBarVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    private void OnDestroy()
    {
        if (_bossEntity != null)
        {
            _bossEntity.OnTakeHit -= UpdateHealthBar;
        }
    }
}