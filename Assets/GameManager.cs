using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;       // assign your GameOver panel here
    public GameObject levelClearedPanel;   // assign your Level Cleared panel here

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (levelClearedPanel != null)
            levelClearedPanel.SetActive(false);
    }

    // ---------------- Score ----------------
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    // ---------------- Lives ----------------
    public void UpdateLivesUI(int currentLives)
    {
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;

        if (currentLives <= 0)
            TriggerGameOver();
    }

    // ---------------- Game Over ----------------
    public void TriggerGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f; // pause game
        StartCoroutine(ReturnToMainMenuAfterDelay(3f));
    }

    // ---------------- Level Cleared ----------------
    public void TriggerLevelCleared()
    {
        if (levelClearedPanel != null)
            levelClearedPanel.SetActive(true);

        Time.timeScale = 0f; // pause game
        StartCoroutine(ReturnToMainMenuAfterDelay(3f));
    }

    private System.Collections.IEnumerator ReturnToMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f; // reset time scale
        SceneManager.LoadScene("Menu"); // make sure your main menu scene is named "Menu"
    }
}
