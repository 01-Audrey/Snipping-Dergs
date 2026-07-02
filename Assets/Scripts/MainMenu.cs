using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ============================================================
// MainMenu.cs
// Purpose: Handles all main menu button functionality
// How to use:
//   1. Attach to an empty GameObject called "MainMenuManager"
//   2. Assign each button in the Inspector
// ============================================================

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button playButton;
    public Button highScoreButton;
    public Button settingsButton;
    public Button exitButton;

    [Header("Panels")]
    public GameObject highScorePanel;   // Optional — assign if you have one
    public GameObject settingsPanel;    // Optional — assign if you have one

    void Start()
    {
        // Connect buttons to functions
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);
        if (highScoreButton != null)
            highScoreButton.onClick.AddListener(OpenHighScore);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);

        // Hide panels at start
        if (highScorePanel != null)
            highScorePanel.SetActive(false);
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        Debug.Log("Loading GameScene...");
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenHighScore()
    {
        Debug.Log("Opening High Score...");
        if (highScorePanel != null)
            highScorePanel.SetActive(true);
    }

    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
