using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxRollSpeed = 8f;
    public float accelerationSpeed = 15f;
    public float decelerationSpeed = 10f;
    public float rollDegreeMultiplier = 1f;
    public float jumpSpeed = 12f;
    public LayerMask groundLayer = 1;

    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private MobileInputHandler mobileInput;

    float diameter;
    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        mobileInput = GetComponent<MobileInputHandler>();
        diameter = 2 * Mathf.PI * cc.radius;
    }

    void Update()
    {
        isGrounded = cc.IsTouchingLayers(groundLayer);

        float horizontalInput = 0f;

        if (mobileInput != null)
        {
            horizontalInput = mobileInput.moveDirection; // -1, 0, 1
            if (mobileInput.jumpPressed)
            {
                Jump();
                mobileInput.jumpPressed = false; // reset after jump
            }
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        Move(horizontalInput);
        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        float speed = rb.linearVelocity.magnitude;
        if (speed > 0)
        {
            transform.Rotate(0, 0, (speed * Mathf.Sign(rb.linearVelocity.x) * Time.deltaTime / diameter) * 360f * rollDegreeMultiplier);
        }
    }

    private void Move(float h)
    {
        if (h != 0)
        {
            // Apply acceleration
            rb.linearVelocity += h * accelerationSpeed * Vector2.right * Time.deltaTime;

            // Clamp to max speed (fixed the condition)
            if (Mathf.Abs(rb.linearVelocity.x) > maxRollSpeed)
            {
                rb.linearVelocity = new Vector2(h * maxRollSpeed, rb.linearVelocity.y);
            }
        }
        else
        {
            // Apply deceleration when no input
            if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
            {
                float deceleration = decelerationSpeed * Time.deltaTime;
                float newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, 0, deceleration);
                rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
            }
        }
    }

    void Jump()
    {
        if (!isGrounded) return;

        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        cc.GetContacts(contacts);

        // Safety check to ensure we have contacts
        if (contacts.Count > 0)
        {
            rb.linearVelocity += contacts[0].normal * jumpSpeed;
        }
        else
        {
            // Fallback jump if no contacts found
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
        }
    }
}