using UnityEngine;
using UnityEngine.UI; // If using Text
// using TMPro; // If using TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public Text scoreText;
    // public TMP_Text scoreText; // if using TextMeshPro

    private int score = 0;

    void Awake()
    {
        // Make singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
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
    [Header("UI References")]
    public Text livesText;

    public void UpdateLivesUI(int currentLives)
    {
        Debug.Log("Lives: " + currentLives);
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;
    }

}
