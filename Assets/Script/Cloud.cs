using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed;
    private float offScreenX;

    void Start()
    {
        offScreenX = (Camera.main.orthographicSize * Screen.width / Screen.height) + 5f;
        if (transform.position.x > 0)
        {
            speed = -Mathf.Abs(speed);
        }
        else
        {
            speed = Mathf.Abs(speed);
        }
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > offScreenX)
        {
            Destroy(gameObject);
        }
    }
}