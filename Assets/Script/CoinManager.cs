// Import the TextMeshPro library to work with UI Text
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    // Drag your UI Text element here in the Inspector
    public TextMeshProUGUI coinText;

    private int coinsCollectedThisRun = 0;

    void Awake()
    {
        // Set up the singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize the UI text with the starting amount
        UpdateCoinText();
    }

    public void AddCoin()
    {
        coinsCollectedThisRun++;
        UpdateCoinText();
    }

    // A dedicated method to update the text display
    private void UpdateCoinText()
    {
        // Display only the number of coins collected
        coinText.text = coinsCollectedThisRun.ToString();
    }
}