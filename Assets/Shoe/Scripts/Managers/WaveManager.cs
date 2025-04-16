using System;
using System.Collections;
using UnityEngine;

public class WaveManager : IWaveManager, IDisposable
{
    private EnemyData[] enemies;
    private WaveData waveData;
    private int currentWave = 0;
    private int totalEnemyCount;
    private int enemyDespawnCount;
    private float waveTimer;
    private bool waveWaitingToBeSpawned = true;

    public float TimeUntilNextWave { get; set; }

    public bool IsLastWave() { return currentWave == waveData.WaveCount; }

    public static event Action AllEnemiesProcessed;
    public static event Action<EnemyCharacter> SpawnedEnemy;
    public event Action NewWaveStarted;

    private GameObject spawnerCoroutineObject;
    private CoroutineMonoBehavior spawnerCoroutine;

    public void Initialize()
    {
        CreateEnemyDatabase();

        spawnerCoroutineObject = new GameObject("Loading Game Object");
        spawnerCoroutine = spawnerCoroutineObject.AddComponent<CoroutineMonoBehavior>();

        waveData = Services.Get<IGameManager>().GetLevelData.WaveData;

        // Start the first wave after set delay
        waveTimer = waveData.DelayBetweenWaves - waveData.FirstWaveStartTimer;

        // Sub to all events
        EnemyCharacter.EnemyArrivedHomeEvent += EnemyDespawned;
        EnemyCharacter.EnemyDied += EnemyKilled;
        HUD.NextWave += AttemptToSpawn;

        // Get total enemy count
        foreach (var wave in waveData.WaveCompositions)
        {
            for (int i = 0; i < wave.EnemyCountPerGroup.Length; i++)
            {
                totalEnemyCount += wave.EnemyCountPerGroup[i];
            }
        }
    }

    /// <summary>
    /// Spawn next wave ahead of time if called from UI and allowed
    /// </summary>
    private void AttemptToSpawn()
    {
        if (waveWaitingToBeSpawned)
            waveTimer = waveData.DelayBetweenWaves;
    }

    /// <summary>
    /// Get a lost of all enemy types
    /// </summary>
    private void CreateEnemyDatabase()
    {
        var _obj = Resources.LoadAll("", typeof(EnemyData));
        enemies = new EnemyData[_obj.Length];
        for (int i = 0; i < _obj.Length; i++)
        {
            enemies[i] = (EnemyData)_obj[i];
        }
    }

    public (int, int) WaveCounter()
    {
        return (currentWave, waveData.WaveCount);
    }

    public void Update()
    {
        // Don't advance timer if all waves are spawned
        if (currentWave >= waveData.WaveCompositions.Length)
            return;

        if (waveWaitingToBeSpawned)
            waveTimer += Time.deltaTime * Time.timeScale;

        if (waveTimer >= waveData.DelayBetweenWaves)
        {
            waveTimer = 0;
            waveWaitingToBeSpawned = false;
            spawnerCoroutine.StartCoroutine(SpawnWave());
        }

        TimeUntilNextWave = waveData.DelayBetweenWaves - waveTimer;
    }

    /// <summary>
    /// Spawn new wave
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWave()
    {
        WaveComposition waveComposition = waveData.WaveCompositions[currentWave];

        currentWave++;
        NewWaveStarted?.Invoke();

        // Iterate over each group in the wave composition
        for (int groupIndex = 0; groupIndex < waveComposition.OrderOfEnemies.Length; groupIndex++)
        {
            EnemyType enemyType = waveComposition.OrderOfEnemies[groupIndex];
            int enemyCount = waveComposition.EnemyCountPerGroup[groupIndex];
            float groupDelay = waveComposition.DelayBetweenGroups[groupIndex];

            // Spawn enemies for this group
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy(enemyType);
                yield return new WaitForSeconds(waveComposition.DelayBetweenUnits[groupIndex]);
            }

            // If there are more groups, wait for the delay between groups
            if (groupIndex < waveComposition.OrderOfEnemies.Length - 1)
            {
                yield return new WaitForSeconds(groupDelay);
            }
        }

        waveWaitingToBeSpawned = true;
    }

    /// <summary>
    /// Spawn an enemy based on the type requested by the spawner
    /// </summary>
    /// <param name="enemyType"></param>
    private void SpawnEnemy(EnemyType enemyType)
    {
        foreach (var requestedEnemy in enemies)
        {
            if (requestedEnemy.EnemyType == enemyType)
            {
                // Randomise spawn position so they don't always spawn on top of each other
                Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1));
                Vector3 spawnPosition = Services.Get<IPathFinder>().EntrancePoint.position + randomVector;

                EnemyCharacter enemy = requestedEnemy.WorldPrefab.GetComponent<EnemyCharacter>().Get<EnemyCharacter>(spawnPosition, Quaternion.Euler(waveData.GetSpawnRotation));
                enemy.SpawnEnemy();
                SpawnedEnemy?.Invoke(enemy);

                break;
            }
        }
    }

    /// <summary>
    /// Event called each time an enemies dies
    /// </summary>
    /// <param name="enemy"></param>
    void EnemyKilled(EnemyData enemy, Vector3 pos)
    {
        if (++enemyDespawnCount >= totalEnemyCount)
        {
            // once all enemies are killed, call the event
            AllEnemiesProcessed?.Invoke();
        }
    }

    /// <summary>
    /// When enemy reaches base, they will be counted as despawned
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void EnemyDespawned(EnemyCharacter obj)
    {
        if (++enemyDespawnCount >= totalEnemyCount)
        {
            // once all enemies are killed, call the event
            AllEnemiesProcessed?.Invoke();
        }
    }

    /// <summary>
    /// Unsubscribe from events
    /// </summary>
    public void Dispose()
    {
        EnemyCharacter.EnemyDied -= EnemyKilled;
    }
}
