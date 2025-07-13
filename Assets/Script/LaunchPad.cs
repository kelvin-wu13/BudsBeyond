using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public float launchForce = 30f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply upward force to the player
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, launchForce);

            // Tell the GameManager to start the main game sequence
            GameManager.instance.StartGameplay();

            // Disable the launchpad after use
            GetComponent<Collider2D>().enabled = false;
        }
    }
}