using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IGameManager
{
    private bool pauseOn;
    private LevelData currentLevelData;
    public LevelData GetLevelData => currentLevelData;

    public Action VictoryEvent { get; set;}
    public Action DefeatEvent { get; set; }

    public event Action GamePaused;
    public event Action GameUnPaused;

    public void Initialize()
    {
        // Set level specific data
        SetLevelData();

        // Subscribe to events
        Services.Get<IInputManager>().EscapeButton += PauseToggle;
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
    }

    private void GameWon(MonoBehaviour reference)
    {
        reference.Invoke(nameof(ToggleWinEvent), 3);
    }

    void ToggleWinEvent()
    {
        VictoryEvent?.Invoke();
        PauseToggle();
    }

    private void PauseToggle()
    {
        pauseOn = !pauseOn;

        if (pauseOn)
            GamePaused?.Invoke();
        else GameUnPaused?.Invoke();
    }

    void Disable()
    {
        Services.Get<IInputManager>().EscapeButton -= PauseToggle;
        GemManager.AllGemsLost -= GameLost;
        WaveManager.AllEnemiesKilled -= GameWon;
        HUD.OnRestartButton -= RestartGame;
    }
}