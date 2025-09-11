using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;       
    public GameObject levelClearedPanel;   

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

    public void UpdateLivesUI(int currentLives)
    {
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;

        if (currentLives <= 0)
            TriggerGameOver();
    }

    public void TriggerGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f; // pause game
        StartCoroutine(ReturnToMainMenuAfterDelay(3f));
    }

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

        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu"); 
    }
}
