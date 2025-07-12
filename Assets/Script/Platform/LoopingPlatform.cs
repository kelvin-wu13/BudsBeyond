using System.Collections;
using UnityEngine;

public class LoopingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the platform moves.")]
    [SerializeField] private float moveSpeed = 3f;

    [Tooltip("The direction of movement. (1, 0) for right, (-1, 0) for left.")]
    [SerializeField] private Vector2 moveDirection = Vector2.right;

    [Header("Looping Points")]
    [Tooltip("The Transform where the platform's leading edge will reappear.")]
    [SerializeField] private Transform spawnPoint;

    [Tooltip("The Transform where the platform's leading edge will trigger the loop.")]
    [SerializeField] private Transform endPoint;

    [Header("Transition Settings")]
    [Tooltip("How long the fade in and fade out animations last.")]
    [SerializeField] private float transitionDuration = 0.5f;

    private Vector3 platformEdgeOffset;
    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    private bool isTransitioning = false;

    void Start()
    {
        // Get components needed for the transition
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();

        if (spawnPoint == null || endPoint == null)
        {
            Debug.LogError("Spawn Point or End Point is not set in the Inspector!", this);
            this.enabled = false;
            return;
        }

        CalculatePlatformEdgeOffset();
    }

    void Update()
    {
        // Only move the platform if it's not currently fading in or out
        if (!isTransitioning)
        {
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }

        // Check if the platform has passed the end point and is not already transitioning
        if (HasPassedEndPoint() && !isTransitioning)
        {
            // Start the fade out -> teleport -> fade in process
            StartCoroutine(TransitionRoutine());
        }
    }

    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;
        platformCollider.enabled = false;

        yield return FadePlatform(1, 0);

        transform.position = spawnPoint.position - platformEdgeOffset;

        yield return FadePlatform(0, 1);

        platformCollider.enabled = true;
        isTransitioning = false;
    }

    private IEnumerator FadePlatform(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / transitionDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null; // Wait for the next frame
        }

        spriteRenderer.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    private void CalculatePlatformEdgeOffset()
    {
        if (platformCollider == null) return;
        Vector2 size = platformCollider.bounds.size;
        Vector2 direction = moveDirection.normalized;
        platformEdgeOffset = new Vector3(size.x / 2f * direction.x, size.y / 2f * direction.y, 0);
    }

    private bool HasPassedEndPoint()
    {
        Vector3 leadingEdgePosition = transform.position + platformEdgeOffset;
        if (moveDirection.x > 0) return leadingEdgePosition.x > endPoint.position.x;
        if (moveDirection.x < 0) return leadingEdgePosition.x < endPoint.position.x;
        if (moveDirection.y > 0) return leadingEdgePosition.y > endPoint.position.y;
        if (moveDirection.y < 0) return leadingEdgePosition.y < endPoint.position.y;
        return false;
    }
}