using UnityEngine;

// ============================================================
// DragonMover.cs
// Dragon walks left-right while moving across screen
// ============================================================

public class DragonMover : MonoBehaviour
{
    [Header("Flight Settings")]
    public float speed = 4f;
    public float wobbleFrequency = 3f;
    public float wobbleAmplitude = 0.4f;

    [Header("Walk Animation")]
    public float tiltAmount = 15f;
    public float tiltSpeed = 8f;

    [Header("Screen Bounds")]
    public float exitBuffer = 2f;

    private float startY;
    private float timeAlive = 0f;
    private bool isDead = false;
    private float moveDirection = 1f;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startY = transform.position.y;

        if (transform.position.x > 0)
            moveDirection = -1f;
        else
            moveDirection = 1f;

        if (sr != null)
            sr.flipX = (moveDirection < 0);
    }

    void Update()
    {
        if (isDead) return;

        timeAlive += Time.deltaTime;

        float newX = transform.position.x + moveDirection * speed * Time.deltaTime;
        float newY = startY + Mathf.Sin(timeAlive * wobbleFrequency) * wobbleAmplitude;

        transform.position = new Vector3(newX, newY, 0f);

        // Tilt left and right to simulate walking
        float tilt = Mathf.Sin(timeAlive * tiltSpeed) * tiltAmount;
        transform.rotation = Quaternion.Euler(0f, 0f, tilt);

        float screenEdge = Camera.main.orthographicSize * Camera.main.aspect + exitBuffer;
        if (Mathf.Abs(transform.position.x) > screenEdge)
        {
            DragonHealth health = GetComponent<DragonHealth>();
            if (health != null)
                health.OnEscaped();
            else
                Destroy(gameObject);
        }
    }

    public void StopFlying()
    {
        isDead = true;
        transform.rotation = Quaternion.identity;
    }
}