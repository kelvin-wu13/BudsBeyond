using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public float launchForce = 30f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, launchForce);

            GameManager.instance.StartGameplay();

            GetComponent<Collider2D>().enabled = false;
        }
    }
}