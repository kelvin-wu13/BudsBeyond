using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform cameraTransform;

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            transform.position = new Vector3(transform.position.x, cameraTransform.position.y, transform.position.z);
        }
    }
}