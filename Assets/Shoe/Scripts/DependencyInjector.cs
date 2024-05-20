using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DependencyInjector : MonoBehaviour
{
    /*
    private InputManager inputManager;
    private Pathfinder pathFinder;
    private GameStateManager gameStateManager;
    private GemManager gemManager;
    //private GameManager gameManager;
    private EconomicManager economicManager;
    private WaveManager waveManager;

    private List<IInputEventSubscriber> inputSubscribers = new List<IInputEventSubscriber>();
    private List<IPathfinderSubscriber> pathfinderSubscribers = new List<IPathfinderSubscriber>();
    private List<IGameStateEventSubscriber> gameStateSubscribers = new List<IGameStateEventSubscriber>();
    private List<IGemManagerSubscriber> gemManagerSubscribers = new List<IGemManagerSubscriber>();
    //private List<IGameManagerSubscriber> gameManagerSubscribers = new List<IGameManagerSubscriber>();
    private List<IEconomicsManagerSubscriber> economicManagerSubscribers = new List<IEconomicsManagerSubscriber>();
    private List<IWaveManagerSubscriber> waveManagerSubscribers = new List<IWaveManagerSubscriber>();

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        gemManager = GetComponent<GemManager>();
        waveManager = GetComponent<WaveManager>();

        //gameManager = new GameManager();
        pathFinder = new Pathfinder();
        gameStateManager = new GameStateManager();
        economicManager = new EconomicManager();

        PopulateSubscribers();
        InjectDependencies();

        pathFinder.Initialize();
       // gameManager.Initialize();
        economicManager.Initialize();
    }

    void PopulateSubscribers()
    {
       // SetListenersForNonMonobehaviours(gameManager);
        SetListenersForNonMonobehaviours(pathFinder);
        SetListenersForNonMonobehaviours(gameStateManager);
        SetListenersForNonMonobehaviours(economicManager);
        SetListenersForNonMonobehaviours(waveManager);

        // Populate Input Subscribers
        foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<IInputEventSubscriber>())
        {
            inputSubscribers.Add(item);
        }

        // Populate Pathfinder Subscribers
        foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<IPathfinderSubscriber>())
        {
            pathfinderSubscribers.Add(item);
        }

        // Populate Game State Event Subscribers
        foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<IGameStateEventSubscriber>())
        {
            gameStateSubscribers.Add(item);
        }

        // Populate Gem Manager Subscribers
        foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<IGemManagerSubscriber>())
        {
            gemManagerSubscribers.Add(item);
        }

        //// Populate Game Manager Subscribers
        //foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<IGameManagerSubscriber>())
        //{
        //    gameManagerSubscribers.Add(item);
        //}

        // Populate Economics Manager Subscribers
        foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<IEconomicsManagerSubscriber>())
        {
            economicManagerSubscribers.Add(item);
        }

        foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<IWaveManagerSubscriber>())
        {
            waveManagerSubscribers.Add(item);
        }
    }


    private void SetListenersForNonMonobehaviours<T>(T value)
    {
        if (value is IInputEventSubscriber)
            inputSubscribers.Add(value as IInputEventSubscriber);

        if (value is IGameStateEventSubscriber)
            gameStateSubscribers.Add(value as IGameStateEventSubscriber);

        if (value is IGemManagerSubscriber)
            gemManagerSubscribers.Add(value as IGemManagerSubscriber);

        //if (value is IGameManagerSubscriber)
        //    gameManagerSubscribers.Add(value as IGameManagerSubscriber);

        if (value is IPathfinderSubscriber)
            pathfinderSubscribers.Add(value as IPathfinderSubscriber);

        if (value is IEconomicsManagerSubscriber)
            economicManagerSubscribers.Add(value as IEconomicsManagerSubscriber);

        if (value is IWaveManagerSubscriber)
            waveManagerSubscribers.Add(value as IWaveManagerSubscriber);
    }

    private void InjectDependencies()
    {
        foreach (var subscriber in inputSubscribers)
        {
            subscriber.SetInputManagerReference(inputManager);
        }

        // Pathfinder Subscribers
        foreach (var subscriber in pathfinderSubscribers)
        {
            subscriber.SetPathfinderReference(pathFinder);
        }

        // Game State Event Subscribers
        foreach (var subscriber in gameStateSubscribers)
        {
            subscriber.SetGameStateManagerReference(gameStateManager);
        }

        // Gem Manager Subscribers
        foreach (var subscriber in gemManagerSubscribers)
        {
            subscriber.SetGemManagerReference(gemManager);
        }

        // Game Manager Subscribers
        //foreach (var subscriber in gameManagerSubscribers)
        //{
        //    subscriber.SetGameManagerReference(gameManager);
        //}

        // Economics Manager Subscribers
        foreach (var subscriber in economicManagerSubscribers)
        {
            subscriber.SetEconomicsManagerReference(economicManager);
        }

        foreach (var subscriber in waveManagerSubscribers)
        {
            subscriber.SetWaveManagerReference(waveManager);
        }
    }
    */
}
