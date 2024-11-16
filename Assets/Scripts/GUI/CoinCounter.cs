using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    public Text coinText;
    private int _coins;

    private void Start()
    {
        _coins = 0;
        coinText.text = _coins.ToString();

        EnemyEntity[] enemies = FindObjectsOfType<EnemyEntity>();
        BossEntity[] enemies1 = FindObjectsOfType<BossEntity>();
        foreach (EnemyEntity enemy in enemies)
        {
            enemy.OnDeath += EnemyEntity_OnDeath;
        }

        foreach (BossEntity enemy in enemies1)
        {
            enemy.OnDeath += BossEntity_OnDeath;
        }

        Player.GetInstance().AddCoins += Player_AddCoins;
    }

    public void RestartCoinCounter()
    {
        _coins = 0;
        UpdateCoinText();
    }

    public int GetCoins()
    {
        return _coins;
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

    private void BossEntity_OnDeath(object sender, EventArgs e)
    {
        if (sender is BossEntity enemy)
        {
            EnemySO enemySO = enemy.GetComponent<BossEntity>()._enemySO;
            _coins += enemySO.enemyDropScore;
            UpdateCoinText();
        }
    }

    private void Player_AddCoins(object sender, EventArgs e)
    {
        _coins += Player.GetInstance().LastCollectedCoinAmount;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = _coins.ToString();
    }
}