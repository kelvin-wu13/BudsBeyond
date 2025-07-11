using UnityEngine;

public class WindmillPlatform : MonoBehaviour
{
    // The speed at which the platform will spin.
    [SerializeField] private float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the platform around its Z-axis.
        // We multiply by Time.deltaTime to make the rotation smooth and independent of the frame rate.
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}