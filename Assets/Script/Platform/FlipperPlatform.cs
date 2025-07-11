using UnityEngine;
using System.Collections;

public class FlipperPlatform : MonoBehaviour
{
    [Header("Flipper Settings")]
    [SerializeField] private float motorSpeed = 1000f;
    [SerializeField] private float flipDuration = 0.3f;

    private HingeJoint2D hinge;
    private JointMotor2D motor;
    private bool isFlipping = false; // Prevents the flipper from activating multiple times

    void Start()
    {
        // Get the HingeJoint2D component attached to this GameObject
        hinge = GetComponent<HingeJoint2D>();

        // Get the existing motor from the hinge to modify it
        motor = hinge.motor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the flipper is already active and if the object that landed is the "Player"
        if (!isFlipping && collision.gameObject.CompareTag("Player"))
        {
            // Check if the player is landing on top of the flipper
            if (collision.contacts[0].normal.y < -0.5f)
            {
                StartCoroutine(ActivateFlipper());
            }
        }
    }


    private IEnumerator ActivateFlipper()
    {
        isFlipping = true;

        // Flip Up
        motor.motorSpeed = -motorSpeed; // Negative speed to rotate up
        hinge.motor = motor;

        // Wait for the specified duration
        yield return new WaitForSeconds(flipDuration);

        // Flip Down
        motor.motorSpeed = motorSpeed; // Positive speed to rotate back down
        hinge.motor = motor;

        // Allow the flipper to be activated again after a short delay
        yield return new WaitForSeconds(0.5f);
        isFlipping = false;
    }
}