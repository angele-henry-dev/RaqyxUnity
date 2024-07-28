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

    void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;
            EventManager.StartListening(EventManager.Event.onStartGame, OnStartGame);
        }
    }

    void Start()
    {
        ResetGame();
    }

    void OnDestroy()
    {
        EventManager.StopListening(EventManager.Event.onStartGame, OnStartGame);
    }

    public void ResetGame()
    {
        Dictionary<string, object> message = new();
        Vector2[] polygonPoints = {
            new Vector2(-2f, 4f),
            new Vector2(2f, 4f),
            new Vector2(2f, -2f),
            new Vector2(-2f, -2f)
        };
        message.Add("polygonPoints", polygonPoints);
        EventManager.TriggerEvent(EventManager.Event.onStartGame, message);
    }

    public void IncreaseScore()
    {
        score++;
        CheckWin();
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
            ResetGame();
        }
    }

    private void OnStartGame(Dictionary<string, object> message=null)
    {
        score = 0;
    }
}
