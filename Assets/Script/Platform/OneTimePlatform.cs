using System.Collections;
using UnityEngine;

public class OneTimeBouncePlatform : MonoBehaviour
{
    [Header("Bounce Settings")]
    [Tooltip("How high the player will be bounced up.")]
    public float bounceForce = 20f;

    [Tooltip("A short delay before the platform destroys itself after bouncing the player.")]
    public float destroyDelay = 0.2f;

    private bool hasBounced = false; // Prevents the platform from bouncing the player multiple times

    // This function is called when another collider makes physical contact with this one.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the platform has already been used and if the object is the player.
        if (!hasBounced && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            // Check if the player is landing on top of the platform (i.e., they are falling).
            if (playerRb != null && playerRb.linearVelocity.y <= 0.1f)
            {
                hasBounced = true;

                // Apply an instant upward force to the player.
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);

                // Start the process to destroy the platform.
                StartCoroutine(DestroyAfterBounce());
            }
        }
    }

    private IEnumerator DestroyAfterBounce()
    {
        // Optional: Trigger an animation or particle effect here.

        // Disable the collider immediately to prevent the player from re-colliding.
        GetComponent<Collider2D>().enabled = false;

        // Wait for the specified delay.
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the platform GameObject.
        Destroy(gameObject);
    }
}