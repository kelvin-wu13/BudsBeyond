using UnityEngine;

public class LaunchPowerUp : MonoBehaviour
{
    [Header("Power-up Settings")]
    [Tooltip("The vertical force applied to the player.")]
    public float launchForce = 25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Try to get the player's Rigidbody2D component
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Apply an instant upward force by setting the velocity.
                // This overrides the current vertical speed but keeps the horizontal speed.
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, launchForce);

                // Destroy the power-up object after it has been used
                Destroy(gameObject);
            }
        }
    }
}