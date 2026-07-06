using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Round Settings")]
    public int dragonsPerRound = 5;
    public int killsRequiredToPass = 3;
    public int maxEscapesAllowed = 3;
    [Header("Score Settings")]
    public int pointsPerKill = 100;
    public int oneShotBonus = 50;
    [Header("References")]
    public DragonSpawner dragonSpawner;
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI escapedText;
    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreDisplayText;
    private int currentScore = 0;
    private int currentRound = 1;
    private int dragonsKilled = 0;
    private int dragonsEscaped = 0;
    private int highScore = 0;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        UpdateUI();
        StartRound();
    }
    void StartRound()
    {
        dragonsKilled = 0;
        dragonsEscaped = 0;
        if (dragonSpawner != null)
            dragonSpawner.StartSpawning();
    }
    public void OnDragonKilled()
    {
        dragonsKilled++;
        currentScore += pointsPerKill;
        UpdateUI();
    }
    public void OnDragonEscaped()
    {
        dragonsEscaped++;
        UpdateUI();
        if (dragonsEscaped >= maxEscapesAllowed)
            GameOver();
    }
    public void AddBonusScore()
    {
        currentScore += oneShotBonus;
        UpdateUI();
    }
    public void UpdateShotDisplay(int shotsRemaining)
    {
        Debug.Log("Shots remaining: " + shotsRemaining);
    }
    private void GameOver()
    {
        if (dragonSpawner != null)
            dragonSpawner.StopSpawning();
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "Score: " + currentScore;
            if (highScoreDisplayText != null)
                highScoreDisplayText.text = "Best: " + highScore;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        Debug.Log("GoToMainMenu clicked!");
        SceneManager.LoadScene("MainMenu");
    }
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
        if (roundText != null)
            roundText.text = "Round: " + currentRound;
        if (highScoreText != null)
            highScoreText.text = "Best: " + highScore;
        if (escapedText != null)
            escapedText.text = "Escaped: " + dragonsEscaped + "/" + maxEscapesAllowed;
    }
}