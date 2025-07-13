using UnityEngine;

public class LaunchPowerUp : MonoBehaviour
{
    [Header("Power-up Settings")]
    [Tooltip("The vertical force applied to the player.")]
    public float launchForce = 25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, launchForce);

                Destroy(gameObject);
            }
        }
    }
}