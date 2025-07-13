using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platform") || other.CompareTag("MovingPlatform") || other.CompareTag("TrapPlatform"))
        {
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            if (CoinManager.instance != null && CurrencySystem.instance != null)
            {
                int coinsFromRun = CoinManager.instance.GetCoinsFromThisRun();
                CurrencySystem.instance.AddCoins(coinsFromRun);
            }
            SceneManager.LoadScene("MainMenu");
        }
    }
}