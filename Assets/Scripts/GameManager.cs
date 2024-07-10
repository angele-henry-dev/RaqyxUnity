using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameUI gameUI;
    public GameAudio gameAudio;
    public Shake screenShake;

    public int score;
    public System.Action onReset;
    public int maxScore = 4;

    public GameMode gameMode = GameMode.NORMAL;

    public enum GameMode
    {
        NORMAL
    }

    public void IncreaseScore()
    {
        score++;
        gameUI.UpdateScore(score);
        gameUI.HighlightScore();
        CheckWin();
    }

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

    private void OnDestroy()
    {
        gameUI.onStartGame -= OnStartGame;
    }

    private void CheckWin()
    {
        if (score == maxScore)
        {
            instance.gameAudio.PlayWinLevelSound();
            gameUI.OnGameEnds();
        } else
        {
            instance.gameAudio.PlayHitPlayerSound();
            onReset?.Invoke();
        }
    }

    private void OnStartGame()
    {
        score = 0;
        gameUI.UpdateScore(score);
    }
}
