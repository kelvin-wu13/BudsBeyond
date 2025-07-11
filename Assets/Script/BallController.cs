using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float maxRollSpeed = 8f;
    [SerializeField] float accelerationSpeed = 15f;
    [SerializeField] float decelerationSpeed = 10f;
    [SerializeField] float rollDegreeMultiplier = 1f;
    [SerializeField] float jumpSpeed = 12f;
    [SerializeField] LayerMask groundLayer = 1;

    private Rigidbody2D rb;
    private CircleCollider2D cc;

    private MobileInputHandler mobileInput;
    private MovingPlatform currentPlatform;

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

        if (currentPlatform != null && isGrounded)
        {
            transform.position += currentPlatform.platformDelta;
        }

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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            currentPlatform = collision.gameObject.GetComponent<MovingPlatform>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            currentPlatform = null;
        }
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
            float newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, 0, decelerationSpeed * Time.deltaTime);
            rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
        }
    }

    void Jump()
    {
        if (!isGrounded) return;

        Vector2 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector2(currentVelocity.x, jumpSpeed);
    }
}