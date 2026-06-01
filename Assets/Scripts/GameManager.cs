using UnityEngine;
using TMPro;

// ============================================================
// GameManager.cs
// Purpose: Central controller — tracks score, rounds, kills,
//          escapes. All other scripts talk to this.
// How to use:
//   1. Attach to an empty GameObject called "GameManager".
//   2. Assign scoreText and roundText in Inspector.
//   3. This uses a Singleton pattern — access via GameManager.Instance
// ============================================================

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   // Singleton — access from any script

    [Header("Round Settings")]
    public int dragonsPerRound = 5;
    public int killsRequiredToPass = 3;

    [Header("Score Settings")]
    public int pointsPerKill = 100;
    public int oneShotBonus = 50;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;
    // Assign shot indicator images in Inspector (3 bullet icons)

    // Internal state
    private int currentScore = 0;
    private int currentRound = 1;
    private int dragonsKilled = 0;
    private int dragonsEscaped = 0;
    private int dragonsSpawnedThisRound = 0;

    void Awake()
    {
        // Singleton setup
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    // Called by DragonHealth when dragon is shot
    public void OnDragonKilled()
    {
        dragonsKilled++;
        dragonsSpawnedThisRound++;
        currentScore += pointsPerKill;
        UpdateUI();
        CheckRoundEnd();
    }

    // Called by DragonMover when dragon exits screen
    public void OnDragonEscaped()
    {
        dragonsEscaped++;
        dragonsSpawnedThisRound++;
        UpdateUI();
        CheckRoundEnd();
    }

    // Called by MouseShoot for one-shot kill bonus
    public void AddBonusScore()
    {
        currentScore += oneShotBonus;
        UpdateUI();
    }

    // Called by MouseShoot to update bullet icons
    public void UpdateShotDisplay(int shotsRemaining)
    {
        // TODO in Week 2: update bullet icon sprites here
        Debug.Log("Shots remaining: " + shotsRemaining);
    }

    private void CheckRoundEnd()
    {
        if (dragonsSpawnedThisRound >= dragonsPerRound)
        {
            if (dragonsKilled >= killsRequiredToPass)
                RoundClear();
            else
                GameOver();
        }
    }

    private void RoundClear()
    {
        Debug.Log("Round " + currentRound + " Clear!");
        currentRound++;
        dragonsKilled = 0;
        dragonsEscaped = 0;
        dragonsSpawnedThisRound = 0;
        // TODO: show RoundClear UI panel, then start next round
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + currentScore);
        // TODO: show GameOver UI panel
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
        if (roundText != null)
            roundText.text = "Round: " + currentRound;
    }
}
