using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public enum GameStatus
    {
        PLAY,
        FAIL,
        END,
        MENU,
        WAIT
    }

    public GameStatus gameStatus;

    private void Start()
    {
        gameStatus = GameStatus.MENU; // Game will start via tap to start.
    }
    private void Update()
    {
        if (gameStatus ==GameStatus.MENU && (Input.touchCount > 0 || Input.GetMouseButtonDown(0))) // If user tap.
        {
            StartGame();
        }
    }
    public void StartGame()
    {
        gameStatus = GameStatus.PLAY;
        UIManager.Instance.tapToStart.SetActive(false);
    }
    public void SetGameStatus(GameStatus status)
    {
        gameStatus = status;
    }
    public void LevelFinishedorFailed() // If level finish or failed show popup
    {
        UIManager.Instance.ShowEndGamePopUp();
    }
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
