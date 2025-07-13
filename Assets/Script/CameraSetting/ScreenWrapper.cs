using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    private Camera mainCamera;
    private float leftEdge;
    private float rightEdge;
    private float buffer = 0.5f;

    void Start()
    {
        mainCamera = Camera.main;

        leftEdge = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - buffer;
        rightEdge = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + buffer;
    }

    void LateUpdate()
    {
        Vector3 newPosition = transform.position;

        if (transform.position.x > rightEdge)
        {
            newPosition.x = leftEdge + buffer;
        }
        else if (transform.position.x < leftEdge)
        {
            newPosition.x = rightEdge - buffer;
        }

        transform.position = newPosition;
    }
}