using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Drag your player into this field in the Inspector

    void LateUpdate()
    {
        // Check if the target (player) is higher than the camera
        if (target.position.y > transform.position.y)
        {
            // Create a new position for the camera that matches the player's Y
            // but keeps the camera's original X and Z
            Vector3 newPosition = new Vector3(transform.position.x, target.position.y, transform.position.z);

            // Apply the new position
            transform.position = newPosition;
        }
    }
}