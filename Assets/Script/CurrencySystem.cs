using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem instance;

    public int TotalCoins { get; private set; }
    private const string COIN_SAVE_KEY = "TotalPlayerCoins";

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load the total coins from device memory
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
            return true; // Purchase successful
        }
        return false; // Not enough coins
    }
}