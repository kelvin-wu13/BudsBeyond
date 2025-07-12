using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null && playerRb.linearVelocity.y <= 0 && other.transform.position.y > transform.position.y)
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage();
                }
            }
        }
    }
}