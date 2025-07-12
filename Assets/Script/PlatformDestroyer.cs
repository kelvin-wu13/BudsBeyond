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
            // Reloads the current scene. Replace with your game over logic if needed.
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage();
            }
        }
    }
}