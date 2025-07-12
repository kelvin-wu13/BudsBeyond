using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object has any of the platform tags
        if (other.CompareTag("Platform") || other.CompareTag("MovingPlatform") || other.CompareTag("TrapPlatform"))
        {
            Destroy(other.gameObject);
        }

        // Check if the object is the player
        if (other.CompareTag("Player"))
        {
            if (CoinManager.instance != null && CurrencySystem.instance != null)
            {
                // Assuming your CoinManager has a public property for coins collected this run
                int coinsFromRun = CoinManager.instance.GetCoinsFromThisRun(); // You'll need to add this function to CoinManager
                CurrencySystem.instance.AddCoins(coinsFromRun);
            }
            SceneManager.LoadScene("MainMenu");
        }
    }
}