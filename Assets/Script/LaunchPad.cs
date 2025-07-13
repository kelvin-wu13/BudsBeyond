using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public float launchForce = 30f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Add this log as the very first line to detect ANY collision.
        Debug.Log("LAUNCHPAD COLLISION DETECTED with object: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            // Add this log to confirm the player was identified correctly.
            Debug.Log("Object was the Player! Calling GameManager to start gameplay...");

            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, launchForce);

            GameManager.instance.StartGameplay();

            GetComponent<Collider2D>().enabled = false;
        }
    }
}