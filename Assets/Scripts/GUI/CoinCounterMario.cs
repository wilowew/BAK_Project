using UnityEngine.UI;
using UnityEngine;

public class CoinCounterMario : MonoBehaviour
{
    public Text coinText;
    public GameObject[] targetObjects;
    private int coins;
    private const string COIN_KEY = "Coins";

    private void Start()
    {
        LoadAndDisplayCoins();
    }

    void Update()
    {
        int missingCount = 0;
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] == null)
            {
                missingCount++;
            }
        }

        int currentCoinCount = coins + (missingCount * 10);
        coinText.text = currentCoinCount.ToString(); 
    }

    private void LoadAndDisplayCoins()
    {
        coins = PlayerPrefs.GetInt(COIN_KEY, 0);
        coinText.text = coins.ToString();
    }
}

