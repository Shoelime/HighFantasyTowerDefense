public class GameServices : Services
{
    protected override void Initialize()
    {
        var inputManager = new InputManager();
        AddService<IInputManager>(inputManager);

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

        var waveManager = new WaveManager();
        AddService<IWaveManager>(waveManager);

        var soundManager = new SoundManager();
        AddService<ISoundManager>(soundManager);

        // Initialize all services
        foreach (var service in serviceMap.Values)
        {
            service.Initialize();
        }
    }
}
