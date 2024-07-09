using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public ScoreText scoreText;
    public GameObject menuObject;

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
}
