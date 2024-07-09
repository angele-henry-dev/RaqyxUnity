using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameUI gameUI;
    public int score;
    public System.Action onReset;
    public int maxScore = 4;

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
            gameUI.onStartGame += OnStartGame;
        }
    }

    public void IncreaseScore()
    {
        score++;
        gameUI.UpdateScore(score);
        gameUI.HighlightScore();
        CheckWin();
    }

    private void CheckWin()
    {
        if (score == maxScore)
        {
            gameUI.OnGameEnds();
        } else
        {
            onReset?.Invoke();
        }
    }

    private void OnStartGame()
    {
        score = 0;
        gameUI.UpdateScore(score);
    }
}
