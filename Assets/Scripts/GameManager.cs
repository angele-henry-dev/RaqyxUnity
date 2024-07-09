using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score;
    public ScoreText scoreText;
    public System.Action onReset;

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void IncreaseScore()
    {
        onReset?.Invoke();

        score++;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.SetScore(score);
    }
}
