using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem instance;

    public int TotalCoins { get; private set; }
    private const string COIN_SAVE_KEY = "TotalPlayerCoins";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        TotalCoins = PlayerPrefs.GetInt(COIN_SAVE_KEY, 0);
    }

    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        PlayerPrefs.SetInt(COIN_SAVE_KEY, TotalCoins);
        PlayerPrefs.Save();
    }

    public bool SpendCoins(int amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= amount;
            PlayerPrefs.SetInt(COIN_SAVE_KEY, TotalCoins);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
}