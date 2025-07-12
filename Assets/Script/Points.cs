using UnityEngine;

public class Points : MonoBehaviour
{
    // This function is automatically called by Unity when another collider enters this one
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched this point has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Call the AddPoint method on the ScoreManager instance
            ScoreManager.instance.AddPoint();

            // Destroy the point GameObject so it can't be collected again
            Destroy(gameObject);
        }
    }
}