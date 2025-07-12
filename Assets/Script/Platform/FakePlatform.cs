using UnityEngine;

public class FakePlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // First, check if the object that entered the trigger is the player.
        if (other.CompareTag("Player"))
        {
            // Get the player's Rigidbody2D component to check their velocity.
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            // NEW: Check if the player exists and if they are falling downwards (velocity.y is negative or zero).
            // This ensures the trigger only activates when being "stepped on" from above.
            if (playerRb != null && playerRb.linearVelocity.y <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}