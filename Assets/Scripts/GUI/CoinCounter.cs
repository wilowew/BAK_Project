using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CoinCounter : MonoBehaviour
{
    public Text coinText;
    public Text finalScoreText;
    private int _coins;
    private const string COIN_KEY = "Coins";

    private void Start()
    {
        LoadCoins();
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
        Player.GetInstance().OnPlayerDeath += Player_OnPlayerDeath;

        if (finalScoreText != null)
        {
            finalScoreText.enabled = false;
        }
    }

    public void HandleCheckpointReached()
    {
        Debug.Log("Переход в финальную сцену на основе очков!");

        if (finalScoreText != null)
        {
            finalScoreText.text = "Ваши финальные очки: " + _coins;
            finalScoreText.enabled = true;
        }

        StartCoroutine(TransitionAfterDelay(5.0f));
    }

    private IEnumerator TransitionAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (_coins > 1000)
        {
            SceneManager.LoadScene("PositiveEndingScene");
        }
        else
        {
            SceneManager.LoadScene("NegativeEndingScene");
        }
    }

    public void RestartCoinCounter()
    {
        ResetCoins();
    }

    public void ResetCoins()
    {
        _coins = 0;
        UpdateCoinText();
        SaveCoins();
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

    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        ResetCoins();
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

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(COIN_KEY, _coins);
        PlayerPrefs.Save();
    }

    private void LoadCoins()
    {
        _coins = PlayerPrefs.GetInt(COIN_KEY, 0);
    }

    private void OnDestroy()
    {
        SaveCoins();
    }
}

