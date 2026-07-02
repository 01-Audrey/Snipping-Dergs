using UnityEngine;
using TMPro;

// ============================================================
// GameManager.cs
// Purpose: Central controller — tracks score, rounds, kills,
//          escapes. Tells DragonSpawner when to start/stop.
// How to use:
//   1. Attach to empty GameObject called "GameManager"
//   2. Drag DragonSpawner object into the spawner slot
//   3. Assign scoreText and roundText (after making HUD)
// ============================================================

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Round Settings")]
    public int dragonsPerRound = 5;
    public int killsRequiredToPass = 3;

    [Header("Score Settings")]
    public int pointsPerKill = 100;
    public int oneShotBonus = 50;

    [Header("References")]
    public DragonSpawner dragonSpawner;   // Drag DragonSpawner object here
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;

    // Internal state
    private int currentScore = 0;
    private int currentRound = 1;
    private int dragonsKilled = 0;
    private int dragonsEscaped = 0;
    private int dragonsSpawnedThisRound = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        StartRound();
    }

    void StartRound()
    {
        dragonsKilled = 0;
        dragonsEscaped = 0;
        dragonsSpawnedThisRound = 0;

        Debug.Log("Round " + currentRound + " started!");

        if (dragonSpawner != null)
            dragonSpawner.StartSpawning();
        else
            Debug.LogError("GameManager: No DragonSpawner assigned!");
    }

    public void OnDragonKilled()
    {
        dragonsKilled++;
        dragonsSpawnedThisRound++;
        currentScore += pointsPerKill;
        Debug.Log("Dragon killed! Score: " + currentScore);
        UpdateUI();
        CheckRoundEnd();
    }

    public void OnDragonEscaped()
    {
        dragonsEscaped++;
        dragonsSpawnedThisRound++;
        Debug.Log("Dragon escaped! Escaped: " + dragonsEscaped);
        UpdateUI();
        CheckRoundEnd();
    }

    public void AddBonusScore()
    {
        currentScore += oneShotBonus;
        Debug.Log("One shot bonus! Score: " + currentScore);
        UpdateUI();
    }

    public void UpdateShotDisplay(int shotsRemaining)
    {
        Debug.Log("Shots remaining: " + shotsRemaining);
    }

    private void CheckRoundEnd()
    {
        if (dragonsSpawnedThisRound >= dragonsPerRound)
        {
            if (dragonSpawner != null)
                dragonSpawner.StopSpawning();

            if (dragonsKilled >= killsRequiredToPass)
                RoundClear();
            else
                GameOver();
        }
    }

    private void RoundClear()
    {
        Debug.Log("Round " + currentRound + " Clear! Score: " + currentScore);
        currentRound++;
        UpdateUI();
        // TODO: show Round Clear UI panel, then call StartRound()
        Invoke("StartRound", 2f); // 2 second delay before next round
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + currentScore);
        // TODO: show Game Over UI panel
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
        if (roundText != null)
            roundText.text = "Round: " + currentRound;
    }
}
