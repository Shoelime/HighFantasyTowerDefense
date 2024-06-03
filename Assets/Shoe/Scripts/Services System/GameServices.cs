public class GameServices : Services
{
    protected override void Initialize()
    {
        var gameManager = new GameManager();
        AddService<IGameManager>(gameManager);

        var pathFinder = new Pathfinder();
        AddService<IPathFinder>(pathFinder);

        var economicsManager = new EconomicManager();
        AddService<IEconomicsManager>(economicsManager);

        var gemManager = new GemManager();
        AddService<IGemManager>(gemManager);

        var gameStateHandler = new GameStateHandler();
        AddService<IGameStateHandler>(gameStateHandler);

        var inputManager = new InputManager();
        AddService<IInputManager>(inputManager);

        var timeManager = new TimeManager();
        AddService<ITimeManager>(timeManager);

        inputManager.Initialize();
        gameManager.Initialize();
        pathFinder.Initialize();
        economicsManager.Initialize();
        gemManager.Initialize();
        gameStateHandler.Initialize();
        timeManager.Initialize();
    }
}
