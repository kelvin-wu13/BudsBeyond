using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Data")]
    [SerializeField] private CharacterData characterData;

    [Header("Shield Settings")]
    [Tooltip("The child GameObject that visually represents the shield.")]
    [SerializeField] private GameObject shieldVisual;

    [Header("Movement Settings")]
    [SerializeField] float maxRollSpeed = 8f;
    [SerializeField] float accelerationSpeed = 15f;
    [SerializeField] float decelerationSpeed = 10f;
    [SerializeField] float airControlMultiplier = 0.7f;
    [SerializeField] float airDecelerationMultiplier = 0.5f;

    [Header("Jump Settings")]
    [SerializeField] float rollDegreeMultiplier = 1f;
    [SerializeField] LayerMask groundLayer = 1;


    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private MobileInputHandler mobileInput;
    private MovingPlatform currentPlatform;

    private bool hasShield = false;
    float diameter;
    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        mobileInput = GetComponent<MobileInputHandler>();
        diameter = 2 * Mathf.PI * cc.radius;

        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }
    }

    public void ActivateShield()
    {
        hasShield = true;
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(true);
        }
    }

    public void TakeDamage()
    {
        if (hasShield)
        {
            hasShield = false;
            if (shieldVisual != null)
            {
                shieldVisual.SetActive(false);
            }
            Debug.Log("Shield protected the player!");
        }
        else
        {
            Debug.Log("Player defeated!");
            Destroy(gameObject);
        }
    }


    void Update()
    {
        isGrounded = cc.IsTouchingLayers(groundLayer);

        float horizontalInput = (mobileInput != null) ? mobileInput.moveDirection : Input.GetAxisRaw("Horizontal");

        if (currentPlatform != null && isGrounded)
        {
            transform.position += currentPlatform.platformDelta;
        }

        Move(horizontalInput);
        AnimationUpdate();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bool isGroundContact = (groundLayer.value & (1 << collision.gameObject.layer)) > 0;
        if (isGroundContact && rb.linearVelocity.y <= 0.1f)
        {
            Jump();
        }

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
        float currentAcceleration = isGrounded ? accelerationSpeed : accelerationSpeed * airControlMultiplier;
        float currentDeceleration = isGrounded ? decelerationSpeed : decelerationSpeed * airDecelerationMultiplier;

        if (h != 0)
        {
            rb.linearVelocity += h * currentAcceleration * Vector2.right * Time.deltaTime;
            if (Mathf.Abs(rb.linearVelocity.x) > maxRollSpeed)
            {
                rb.linearVelocity = new Vector2(h * maxRollSpeed, rb.linearVelocity.y);
            }
        }
        else
        {
            float newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, 0, currentDeceleration * Time.deltaTime);
            rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
        }
    }

    void Jump()
    {
        if (characterData == null) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, characterData.jumpSpeed);
    }

    public CharacterData GetCharacterData()
    {
        return characterData;
    }
}