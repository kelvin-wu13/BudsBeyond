using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    private Camera mainCamera;
    private float leftEdge;
    private float rightEdge;
    private float buffer = 0.5f; // A small buffer to ensure the player is fully off-screen

    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;

        // Calculate screen edges in world units
        // We get the left edge (x=0) and right edge (x=Screen.width) and convert them to world coordinates
        leftEdge = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - buffer;
        rightEdge = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + buffer;
    }

    void LateUpdate()
    {
        // Get the current position
        Vector3 newPosition = transform.position;

        // Check if the player has gone past the right edge
        if (transform.position.x > rightEdge)
        {
            // Move the player to the left edge
            newPosition.x = leftEdge + buffer;
        }
        // Check if the player has gone past the left edge
        else if (transform.position.x < leftEdge)
        {
            // Move the player to the right edge
            newPosition.x = rightEdge - buffer;
        }

        // Apply the new position
        transform.position = newPosition;
    }
}