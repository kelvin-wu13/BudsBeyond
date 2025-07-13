using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    // We've renamed the variable for clarity
    public Transform cameraTransform;

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // The logic is the same, but now it uses the camera's position
            transform.position = new Vector3(transform.position.x, cameraTransform.position.y, transform.position.z);
        }
    }
}