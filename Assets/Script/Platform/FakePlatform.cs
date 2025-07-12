using UnityEngine;

public class FakePlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null && playerRb.linearVelocity.y <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}