using UnityEngine;
using System.Collections;
using System;

// ============================================================
// DragonHealth.cs
// Supports different HP per dragon type:
//   Green - 1 hit, regular speed
//   White - 1 hit, fast
//   Red   - 3 hits, tank
// ============================================================

public class DragonHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1;             // Set in Inspector: Green=1, White=1, Red=3

    [Header("Death Sprite")]
    public Sprite deathSprite;            // Assign matching death sprite in Inspector

    [Header("Death Settings")]
    public float flashDuration = 0.1f;
    public float fallTime = 1.5f;         // How long to fall before disappearing
    public Color hitFlashColor = Color.red;

    // Event — tells DragonSpawner this dragon is done
    public Action OnDragonDone;

    private int currentHealth;
    private bool isDead = false;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        currentHealth = maxHealth;
        sr  = GetComponent<SpriteRenderer>();
        rb  = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb != null)
            rb.gravityScale = 0f;
    }

    // Called by MouseShoot on click
    public void TakeHit()
    {
        if (isDead) return;

        currentHealth--;
        Debug.Log("Dragon hit! HP remaining: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Still alive — flash to show damage
            StartCoroutine(HitFlash());
        }
    }

    private IEnumerator HitFlash()
    {
        if (sr != null)
        {
            sr.color = hitFlashColor;
            yield return new WaitForSeconds(flashDuration);
            sr.color = Color.white;
        }
    }

    private void Die()
    {
        isDead = true;

        // Disable collider so it can't be clicked again
        if (col != null)
            col.enabled = false;

        // Stop flying
        DragonMover mover = GetComponent<DragonMover>();
        if (mover != null)
            mover.StopFlying();

        // Notify GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.OnDragonKilled();

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Flash red
        if (sr != null)
        {
            sr.color = hitFlashColor;
            yield return new WaitForSeconds(flashDuration);
            sr.color = Color.white;
        }

        // Swap to death sprite
        if (sr != null && deathSprite != null)
            sr.sprite = deathSprite;

        // Enable gravity — dragon falls
        if (rb != null)
        {
            rb.gravityScale = 2f;
            rb.linearVelocity = Vector2.zero;
        }

        // Wait then disappear
        yield return new WaitForSeconds(fallTime);

        OnDragonDone?.Invoke();
        Destroy(gameObject);
    }

    // Called by DragonMover when dragon exits screen
    public void OnEscaped()
    {
        if (isDead) return;

        if (GameManager.Instance != null)
            GameManager.Instance.OnDragonEscaped();

        OnDragonDone?.Invoke();
        Destroy(gameObject);
    }
}
