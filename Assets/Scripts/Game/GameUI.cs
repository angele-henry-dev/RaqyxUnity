using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private GameObject menuObject;
    [SerializeField]
    private GameObject quitButton;
    [SerializeField]
    private TMPro.TextMeshProUGUI winText;
    [SerializeField]
    private TMPro.TextMeshProUGUI volumeValueText;

    private void Start()
    {
        CheckDisableQuitButton();
    }

    private void CheckDisableQuitButton()
    {
#if UNITY_EDITOR
        quitButton.SetActive(false);
#endif
    }

    public void OnStartGameButtonClicked()
    {
        menuObject.SetActive(false);
        EventManager.TriggerEvent(EventManager.Event.onStartGame, null);
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
