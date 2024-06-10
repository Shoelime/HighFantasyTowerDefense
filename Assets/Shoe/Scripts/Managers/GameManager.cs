using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IGameManager, IDisposable
{
    private bool pauseOn;
    private LevelData currentLevelData;
    public LevelData GetLevelData => currentLevelData;
    public Action VictoryEvent { get; set;}
    public Action DefeatEvent { get; set; }

    public event Action GamePaused;
    public event Action GameUnPaused;

    public GameManager(IInputManager inputManager) 
    {
        // Set level specific data
        SetLevelData();

        // Subscribe to events
        inputManager.EscapeButton += PauseToggle;
        GemManager.AllGemsLost += GameLost;
        WaveManager.AllEnemiesKilled += GameWon;
        HUD.OnRestartButton += RestartGame;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Set level data for this scene
    /// </summary>
    private void SetLevelData()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string levelNumber = sceneName.Replace("Level", "").Trim();

        LevelData[] allLevelData = Resources.LoadAll<LevelData>("");
        currentLevelData = allLevelData.FirstOrDefault(level => level.name.Contains(levelNumber));
    }

    private void GameLost()
    {
        DefeatEvent?.Invoke();
        PauseToggle();
        Services.Get<GameStateHandler>().SetGameState(GameState.Defeat);
        Services.Get<TimeManager>().SetTimeScale(0.0000001f);
    }

    private void GameWon()
    {
        VictoryEvent?.Invoke();
        PauseToggle();
        Services.Get<GameStateHandler>().SetGameState(GameState.Victory);
        Services.Get<TimeManager>().SetTimeScale(0.0000001f);
    }

    private void PauseToggle()
    {
        pauseOn = !pauseOn;

        if (pauseOn)
        {
            GamePaused?.Invoke();
            Services.Get<TimeManager>().SetTimeScale(0.0000001f);
        }
        else
        {
            GameUnPaused?.Invoke();
            Services.Get<TimeManager>().SetTimeScale(1f);
        }
    }

    public void Dispose()
    {
        Services.Get<IInputManager>().EscapeButton -= PauseToggle;
        GemManager.AllGemsLost -= GameLost;
        WaveManager.AllEnemiesKilled -= GameWon;
        HUD.OnRestartButton -= RestartGame;
    }
}