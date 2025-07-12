using UnityEngine;

public class Coin : MonoBehaviour
{
    // This function is automatically called by Unity when another collider enters this one
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched this coin has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Call the AddCoin method on the CoinManager instance
            CoinManager.instance.AddCoin();

            // Destroy the coin GameObject so it can't be collected again
            Destroy(gameObject);
        }
    }
}