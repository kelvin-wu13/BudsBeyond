using System.Collections;
using UnityEngine;

public class OneTimeBouncePlatform : MonoBehaviour
{
    [Header("Bounce Settings")]
    [Tooltip("How high the player will be bounced up.")]
    public float bounceForce = 20f;

    [Tooltip("A short delay before the platform destroys itself after bouncing the player.")]
    public float destroyDelay = 0.2f;

    private bool hasBounced = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasBounced && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null && playerRb.linearVelocity.y <= 0.1f)
            {
                hasBounced = true;

                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);

                StartCoroutine(DestroyAfterBounce());
            }
        }
    }

    private IEnumerator DestroyAfterBounce()
    {
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}