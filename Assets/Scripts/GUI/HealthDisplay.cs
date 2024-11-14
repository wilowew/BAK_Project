using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Player player; 
    public Image healthBar;
    public Text healthText;

    private void Start()
    {
        player.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Update()
    {
        healthBar.fillAmount = (float)player._currentHealth / player._maxHealth;
        healthText.text = $"{player._currentHealth}/{player._maxHealth}";
    }

    public void ShowHealthDisplay()
    {
        gameObject.SetActive(true);
    }

    public void UpdateHealthDisplay()
    {
        healthBar.fillAmount = (float)player._currentHealth / player._maxHealth;
        healthText.text = $"{player._currentHealth}/{player._maxHealth}";
    }

    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }
}
