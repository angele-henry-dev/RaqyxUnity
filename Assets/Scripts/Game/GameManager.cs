using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Refs")]
    public GameUI gameUI;
    public GameAudio gameAudio;
    public Shake screenShake;

    [Header("Config")]
    public int score = 0;
    public int maxScore = 1;

    public GameMode gameMode = GameMode.NORMAL;

    public enum GameMode
    {
        NORMAL
    }

    public void IncreaseScore()
    {
        score++;
        CheckWin();
    }

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
            EventManager.StartListening(EventManager.Event.onStartGame, OnStartGame);
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventManager.Event.onStartGame, OnStartGame);
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
            EventManager.TriggerEvent(EventManager.Event.onReset, null);
        }
    }

    private void OnStartGame(Dictionary<string, object> message=null)
    {
        score = 0;
    }
}
