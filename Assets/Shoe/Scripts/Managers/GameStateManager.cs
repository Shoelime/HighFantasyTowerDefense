public class GameStateHandler : IGameStateHandler
{
    private GameState currentGameState;
    public GameState GetCurrentGameState => currentGameState;

    public GameStateHandler()
    {
        SetGameState(GameState.Normal);
    }

    public void SetGameState(GameState stateToUse)
    {
        currentGameState = stateToUse;
    }
}

public enum GameState
{
    None,
    Normal,
    Preparation,
    Paused,
    Victory,
    Defeat
}
