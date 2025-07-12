using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Call the public method on the BallController to activate the shield
                playerController.ActivateShield();

                // Destroy the power-up object after it has been collected
                Destroy(gameObject);
            }
        }
    }
}