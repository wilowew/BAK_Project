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
    private const string INITIAL_COIN_KEY = "InitialCoins";
    private const string CHECKPOINT_COIN_KEY = "CheckpointCoins";
    private string currentSceneName;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;

        ResetCheckpointData();

        SetInitialCoins();

        LoadCoins();
        coinText.text = _coins.ToString();

        EnemyEntity[] enemies = FindObjectsOfType<EnemyEntity>();
        BossEntity[] bosses = FindObjectsOfType<BossEntity>();

        foreach (EnemyEntity enemy in enemies)
        {
            enemy.OnDeath += EnemyEntity_OnDeath;
        }

        foreach (BossEntity boss in bosses)
        {
            boss.OnDeath += BossEntity_OnDeath;
        }

        Player.GetInstance().AddCoins += Player_AddCoins;
        Player.GetInstance().OnPlayerDeath += Player_OnPlayerDeath;

        if (finalScoreText != null)
        {
            finalScoreText.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetCoins(0);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SetCoins(3333);
        }
    }

    private void SetCoins(int amount)
    {
        _coins = amount;
        UpdateCoinText();
        SaveCoins();
    }

    private void SetInitialCoins()
    {
        if (PlayerPrefs.HasKey(COIN_KEY))
        {
            _coins = PlayerPrefs.GetInt(COIN_KEY);
        }
        else
        {
            _coins = 0;
        }
    }

    private void ResetCheckpointData()
    {
        PlayerPrefs.DeleteKey(CHECKPOINT_COIN_KEY);
        PlayerPrefs.Save();
    }

    public void SaveCoinsToCheckpoint()
    {
        PlayerPrefs.SetInt(CHECKPOINT_COIN_KEY, _coins);
        PlayerPrefs.Save();
    }

    private void LoadCoinsFromCheckpoint()
    {
        if (PlayerPrefs.HasKey(CHECKPOINT_COIN_KEY))
        {
            _coins = PlayerPrefs.GetInt(CHECKPOINT_COIN_KEY);
        }
        else
        {
            LoadInitialCoins();
        }
        UpdateCoinText();
    }

    private void LoadInitialCoins()
    {
        if (PlayerPrefs.HasKey(COIN_KEY))
        {
            _coins = PlayerPrefs.GetInt(COIN_KEY);
        }
        else
        {
            ResetCoins();
        }
    }

    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        Debug.Log("CoinCounter не загружает количество монет с чекпоинта (Всё норм, это чтобы убрать лишнюю загрузку с CoinCounter)");
    }

    public void HandleCheckpointReached()
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = "Итоговое количество монет: " + _coins;
            finalScoreText.enabled = true;
        }

        StartCoroutine(TransitionAfterDelay(5.0f));
    }

    private IEnumerator TransitionAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (_coins > 3000)
        {
            SceneManager.LoadScene("PositiveEndingScene");
        }
        else
        {
            SceneManager.LoadScene("NegativeEndingScene");
        }
    }

    private void UpdateCoinText()
    {
        coinText.text = _coins.ToString();
    }

    public int GetCoins()
    {
        return _coins;
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
        if (sender is BossEntity boss)
        {
            EnemySO enemySO = boss.GetComponent<BossEntity>()._enemySO;
            _coins += enemySO.enemyDropScore;
            UpdateCoinText();
        }
    }

    private void Player_AddCoins(object sender, EventArgs e)
    {
        _coins += Player.GetInstance().LastCollectedCoinAmount;
        UpdateCoinText();
    }

    public void RestartCoinCounter()
    {
        ResetCoins();
    }

    public void ResetCoins()
    {
        _coins = 0;
        UpdateCoinText();
        PlayerPrefs.SetInt(COIN_KEY, _coins);
        PlayerPrefs.Save();
    }

    private void SaveCoinsToGlobalKey()
    {
        PlayerPrefs.SetInt(COIN_KEY, _coins);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        SaveCoinsToGlobalKey();
    }

    private void OnApplicationQuit()
    {
        SaveCoinsToGlobalKey();
    }
}

