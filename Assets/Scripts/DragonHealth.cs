using UnityEngine;
using System.Collections;

// ============================================================
// DragonHealth.cs
// Assigned to: Fonzy Estrellado
// Purpose: Handles hit detection and death for the dragon.
//          When shot, dragon stops flying, falls under gravity,
//          plays death animation, then despawns.
// How to use:
//   1. Attach this script to the Dragon prefab.
//   2. Assign deathSprite in the Inspector (Death Derg Sprites).
//   3. Make sure Dragon prefab has a Rigidbody2D and Collider2D.
//   4. MouseShoot.cs calls TakeHit() when player clicks the dragon.
// ============================================================

public class DragonHealth : MonoBehaviour
{
    [Header("Death Settings")]
    public Sprite deathSprite;            // Drag "Death Derg Sprites.png" here in Inspector
    public float fallDelay = 0.15f;       // Short pause before gravity kicks in
    public float destroyAfter = 2f;       // How long after death before despawning

    [Header("Flash Effect")]
    public float flashDuration = 0.1f;    // How long the hit flash lasts
    public Color hitFlashColor = Color.red;

    private bool isHit = false;           // Prevent being hit twice
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        sr  = GetComponent<SpriteRenderer>();
        rb  = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Make sure gravity is OFF while flying
        if (rb != null)
            rb.gravityScale = 0f;
    }

    // --------------------------------------------------------
    // TakeHit() — called by MouseShoot.cs on successful click
    // --------------------------------------------------------
    public void TakeHit()
    {
        if (isHit) return;  // Already dead, ignore extra clicks
        isHit = true;

        // Stop the dragon from flying
        DragonMover mover = GetComponent<DragonMover>();
        if (mover != null)
            mover.StopFlying();

        // Disable collider so it can't be clicked again
        if (col != null)
            col.enabled = false;

        // Notify GameManager — dragon was killed
        if (GameManager.Instance != null)
            GameManager.Instance.OnDragonKilled();

        // Start death sequence
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // 1. Flash red to show hit
        if (sr != null)
        {
            sr.color = hitFlashColor;
            yield return new WaitForSeconds(flashDuration);
            sr.color = Color.white;
        }

        // 2. Swap to death sprite
        if (sr != null && deathSprite != null)
            sr.sprite = deathSprite;

        // 3. Brief pause then enable gravity so dragon falls
        yield return new WaitForSeconds(fallDelay);
        if (rb != null)
        {
            rb.gravityScale = 2f;   // Adjust fall speed here
            rb.linearVelocity = Vector2.zero;
        }

        // 4. Destroy after falling
        yield return new WaitForSeconds(destroyAfter);
        Destroy(gameObject);
    }
}
