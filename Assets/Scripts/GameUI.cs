using UnityEngine;

public class GameUI : MonoBehaviour
{
    public ScoreText scoreText;
    public GameObject menuObject;
    public TMPro.TextMeshProUGUI winText;
    public TMPro.TextMeshProUGUI volumeValueText;

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

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        volumeValueText.text = $"{Mathf.RoundToInt(value * 100)} %";
    }
}
