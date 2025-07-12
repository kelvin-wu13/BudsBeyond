using UnityEngine;

public class UpDownPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    public float waitTime = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingToTarget = true;
    private bool waiting = false;

    [HideInInspector] public Vector3 platformDelta;

    private Vector3 lastPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * moveDistance;
        lastPosition = transform.position;
        StartCoroutine(MovePlatform());
    }

    void Update()
    {
        platformDelta = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    System.Collections.IEnumerator MovePlatform()
    {
        while (true)
        {
            if (!waiting)
            {
                transform.position = Vector3.MoveTowards(transform.position, movingToTarget ? targetPosition : startPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, movingToTarget ? targetPosition : startPosition) < 0.01f)
                {
                    waiting = true;
                    yield return new WaitForSeconds(waitTime);
                    movingToTarget = !movingToTarget;
                    waiting = false;
                }
            }
            yield return null;
        }
    }
}
