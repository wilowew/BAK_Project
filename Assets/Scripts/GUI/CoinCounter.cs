using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public Text coinText;
    private int _coins;

    private EnemyEntity enemyEntity;

    private void Start()
    {
        _coins = 0;
        coinText.text = "Очки: " + _coins;
        EnemyEntity[] enemies = FindObjectsOfType<EnemyEntity>();

        foreach (EnemyEntity enemy in enemies)
        {
            enemy.OnDeath += EnemyEntity_OnDeath;
        }
        Player.Instance.AddCoins += Player_AddCoins;
    }

    private void EnemyEntity_OnDeath(object sender, EventArgs e)
    {
        if (sender is EnemyEntity enemy)
        {
            EnemySO enemySO = enemy.GetComponent<EnemyEntity>()._enemySO;

            _coins += enemySO.enemyDropScore; 
            UpdateCoinText();
        }
    }

    private void Player_AddCoins(object sender, EventArgs e)
    {
        _coins = Player.Instance.coins;
        UpdateCoinText(); 
    }

    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        
    }

    private void UpdateCoinText()
    {
        coinText.text = "Очки: " + _coins; 
    }
}
