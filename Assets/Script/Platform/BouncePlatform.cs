using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    // The speed with which the player will be bounced up.
    [SerializeField] float bounceSpeed = 20f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the Player
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRigidbody != null)
            {
                // Check if the player is hitting the platform from above
                if (collision.contacts[0].normal.y < -0.5f)
                {
                    // This avoids conflicts with the BallController's own physics handling.
                    playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, bounceSpeed);
                }
            }
        }
    }
}