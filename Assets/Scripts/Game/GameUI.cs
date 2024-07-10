using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private GameObject menuObject;
    [SerializeField]
    private GameObject quitButton;
    [SerializeField]
    private ScoreText scoreText;
    [SerializeField]
    private TMPro.TextMeshProUGUI winText;
    [SerializeField]
    private TMPro.TextMeshProUGUI volumeValueText;

    public System.Action onStartGame;

    private void Start()
    {
        CheckDisableQuitButton();
    }

    private void CheckDisableQuitButton()
    {
#if UNITY_WEBGL
        quitButton.SetActive(false);
#endif
    }

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

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        volumeValueText.text = $"{Mathf.RoundToInt(value * 100)} %";
    }

    public void OnGameEnds()
    {
        menuObject.SetActive(true);
        winText.text = "Level won!";
    }
}
