using UnityEngine;

// ============================================================
// DragonMover.cs
// Assigned to: Dane Audrey
// Purpose: Controls dragon flight movement across the screen.
//          Dragon enters from left or right, flies with a
//          sine wave vertical wobble, despawns on exit.
// How to use:
//   1. Attach this script to the Dragon prefab.
//   2. Set speed, wobbleFrequency, wobbleAmplitude in Inspector.
//   3. The DragonSpawner will set moveDirection before spawning.
// ============================================================

public class DragonMover : MonoBehaviour
{
    [Header("Flight Settings")]
    public float speed = 3f;              // How fast the dragon moves horizontally
    public float wobbleFrequency = 2f;    // How fast it bobs up and down
    public float wobbleAmplitude = 0.5f;  // How far it bobs up and down

    [Header("Direction")]
    public float moveDirection = 1f;      // 1 = left to right, -1 = right to left

    [Header("Screen Bounds")]
    public float exitBuffer = 1.5f;       // How far past screen edge before despawning

    private float startY;                 // Starting Y position (for wobble reference)
    private float timeAlive = 0f;         // Tracks time for sine wave
    private bool isDead = false;          // Set by DragonHealth when shot

    private SpriteRenderer sr;

    void Start()
    {
        startY = transform.position.y;
        sr = GetComponent<SpriteRenderer>();

        // Flip sprite based on direction
        if (sr != null)
            sr.flipX = (moveDirection < 0);
    }

    void Update()
    {
        // Stop moving if dead (DragonHealth takes over)
        if (isDead) return;

        timeAlive += Time.deltaTime;

        // Horizontal movement
        float newX = transform.position.x + moveDirection * speed * Time.deltaTime;

        // Vertical sine wave wobble
        float newY = startY + Mathf.Sin(timeAlive * wobbleFrequency) * wobbleAmplitude;

        transform.position = new Vector3(newX, newY, transform.position.z);

        // Check if dragon has exited the screen
        float screenEdge = Camera.main.orthographicSize * Camera.main.aspect + exitBuffer;
        if (Mathf.Abs(transform.position.x) > screenEdge)
        {
            OnDragonEscaped();
        }
    }

    // Called by DragonHealth when the dragon is shot
    public void StopFlying()
    {
        isDead = true;
    }

    private void OnDragonEscaped()
    {
        // Notify GameManager that this dragon escaped
        if (GameManager.Instance != null)
            GameManager.Instance.OnDragonEscaped();

        Destroy(gameObject);
    }
}
