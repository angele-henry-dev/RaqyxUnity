using UnityEngine;

public class GameUI : MonoBehaviour
{
    public ScoreText scoreText;
    public GameObject menuObject;
    public TMPro.TextMeshProUGUI winText;

    public System.Action onStartGame;

    public void UpdateScore(int score)
    {
        scoreText.SetScore(score);
    }

    public void HighlightScore()
    {
        scoreText.Highlight();
    }

    public void OnStartGameButtonClicked()
    {
        menuObject.SetActive(false);
        onStartGame?.Invoke();
    }

    public void OnGameEnds()
    {
        menuObject.SetActive(true);
        winText.text = "Level won!";
    }
}
