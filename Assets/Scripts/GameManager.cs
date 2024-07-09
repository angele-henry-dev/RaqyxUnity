using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameUI gameUI;
    public int score;
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
        gameUI.UpdateScore(score);
        gameUI.HighlightScore();
    }
}
