using UnityEngine;

// ============================================================
// DragonMover.cs
// Purpose: Dragon spawns from left or right edge and flies
//          across the screen with a sine wave wobble.
//          Works with Unity's Orthographic 2D camera.
// ============================================================

public class DragonMover : MonoBehaviour
{
    [Header("Flight Settings")]
    public float speed = 3f;
    public float wobbleFrequency = 2f;
    public float wobbleAmplitude = 0.4f;

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

        // Determine direction based on spawn X position
        if (transform.position.x > 0)
            moveDirection = -1f;  // Spawn right, move left
        else
            moveDirection = 1f;   // Spawn left, move right

        // Flip sprite based on direction
        if (sr != null)
            sr.flipX = (moveDirection < 0);
    }

    void Update()
    {
        if (isDead) return;

        timeAlive += Time.deltaTime;

        // Horizontal movement
        float newX = transform.position.x + moveDirection * speed * Time.deltaTime;

        // Sine wave vertical wobble
        float newY = startY + Mathf.Sin(timeAlive * wobbleFrequency) * wobbleAmplitude;

        transform.position = new Vector3(newX, newY, 0f);

        // Check if exited screen
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
    }
}
