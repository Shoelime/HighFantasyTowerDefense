public class GameStateHandler : IGameStateHandler
{
    private GameState currentGameState;
    public GameState GetCurrentGameState => currentGameState;

    public void Initialize()
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
    Victory
}
