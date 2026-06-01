using UnityEngine;

// ============================================================
// MouseShoot.cs
// Purpose: Detects mouse clicks and checks if a dragon was hit.
//          Manages shots remaining per dragon.
// How to use:
//   1. Attach to an empty GameObject called "ShootManager".
//   2. Set maxShotsPerDragon in Inspector (default 3).
//   3. Make sure Dragon prefabs have a Collider2D and DragonHealth.
// ============================================================

public class MouseShoot : MonoBehaviour
{
    [Header("Shot Settings")]
    public int maxShotsPerDragon = 3;

    private int shotsRemaining;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        ResetShots();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shotsRemaining <= 0) return;

        shotsRemaining--;

        // Convert mouse position to world position
        Vector2 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Check if we hit a dragon
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null && hit.CompareTag("Dragon"))
        {
            // HIT — tell the dragon it was shot
            DragonHealth health = hit.GetComponent<DragonHealth>();
            if (health != null)
                health.TakeHit();

            // Check if it was a one-shot kill for bonus score
            bool oneShotKill = (shotsRemaining == maxShotsPerDragon - 1);
            if (oneShotKill && GameManager.Instance != null)
                GameManager.Instance.AddBonusScore();

            ResetShots();
        }
        else
        {
            // MISS
            if (shotsRemaining <= 0)
            {
                // All shots used, dragon escapes
                OnShotsExpired();
            }
        }

        // Update HUD shot display
        if (GameManager.Instance != null)
            GameManager.Instance.UpdateShotDisplay(shotsRemaining);
    }

    public void ResetShots()
    {
        shotsRemaining = maxShotsPerDragon;
        if (GameManager.Instance != null)
            GameManager.Instance.UpdateShotDisplay(shotsRemaining);
    }

    private void OnShotsExpired()
    {
        // Shots ran out — tell GameManager dragon escaped
        // DragonMover will also fire this when it exits screen
        // GameManager handles duplicates safely
    }
}
