using UnityEngine;
using UnityEngine.InputSystem;

// ============================================================
// MouseShoot.cs
// Fixed for Unity 6 New Input System
// ============================================================

public class MouseShoot : MonoBehaviour
{
    [Header("Shot Settings")]
    public int maxShotsPerDragon = 3;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip gunshotClip;

    private int shotsRemaining;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        ResetShots();
    }

    void Update()
    {
        // Unity 6 New Input System
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shotsRemaining <= 0) return;

        shotsRemaining--;

        if (audioSource != null && gunshotClip != null)
            audioSource.PlayOneShot(gunshotClip);

        // Get mouse position using new Input System
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);

        // Check if we hit a dragon
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null && hit.CompareTag("Dragon"))
        {
            Debug.Log("HIT dragon!");
            DragonHealth health = hit.GetComponent<DragonHealth>();
            if (health != null)
                health.TakeHit();

            bool oneShotKill = (shotsRemaining == maxShotsPerDragon - 1);
            if (oneShotKill && GameManager.Instance != null)
                GameManager.Instance.AddBonusScore();

            ResetShots();
        }
        else
        {
            Debug.Log("Miss! Shots remaining: " + shotsRemaining);
            if (shotsRemaining <= 0)
                ResetShots();
        }

        if (GameManager.Instance != null)
            GameManager.Instance.UpdateShotDisplay(shotsRemaining);
    }

    public void ResetShots()
    {
        shotsRemaining = maxShotsPerDragon;
    }
}