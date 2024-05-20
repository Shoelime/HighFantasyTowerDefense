public interface IGameStateHandler : IService
{
    GameState GetCurrentGameState { get; }

    void SetGameState(GameState stateToUse);
}
