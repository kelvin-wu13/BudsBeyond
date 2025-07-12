using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public TextMeshProUGUI coinText;

    private int coinsCollectedThisRun = 0;

    void Awake()
    {
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
        UpdateCoinText();
    }

    public void AddCoin()
    {
        coinsCollectedThisRun++;
        UpdateCoinText();
    }

    public int GetCoinsFromThisRun()
    {
        return coinsCollectedThisRun;
    }

    private void UpdateCoinText()
    {
        coinText.text = coinsCollectedThisRun.ToString();
    }
}