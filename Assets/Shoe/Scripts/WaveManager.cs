using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveData waveData;

    private EnemyData[] enemies;

    private int currentWave = 0;
    private int totalEnemyCount;
    private float waveTimer;

    private void Start()
    {
        CreateEnemyDatabase();
        waveTimer = waveData.DelayBetweenWaves - 2;

        foreach (var wave in waveData.WaveCompositions)
        {
            for (int i = 0; i < wave.EnemyCountPerGroup.Length; i++)
            {
                totalEnemyCount += wave.EnemyCountPerGroup[i];
            }
        }

        Debug.Log("total amount of enemies is " + totalEnemyCount);
    }

    private void CreateEnemyDatabase()
    {
        var _obj = Resources.LoadAll("", typeof(EnemyData));
        enemies = new EnemyData[_obj.Length];
        for (int i = 0; i < _obj.Length; i++)
        {
            enemies[i] = (EnemyData)_obj[i];
        }
    }

    private void Update()
    {
        if (currentWave >= waveData.WaveCompositions.Length)
            return;

        waveTimer += Time.deltaTime * Time.timeScale;

        if (waveTimer >= waveData.DelayBetweenWaves)
        {
            waveTimer = 0;
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        Debug.Log("Spawn wave " + currentWave);
        WaveComposition waveComposition = waveData.WaveCompositions[currentWave];

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

        currentWave++;
    }

    private void SpawnEnemy(EnemyType enemyType)
    {
        foreach (var requestedEnemy in enemies)
        {
            if (requestedEnemy.EnemyType == enemyType)
            {
                Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1));
                Vector3 spawnPosition = Services.Get<IPathFinder>().EntrancePoint.position + randomVector;

                EnemyCharacter enemy = requestedEnemy.WorldPrefab.GetComponent<EnemyCharacter>().Get<EnemyCharacter>(spawnPosition, Quaternion.Euler(waveData.GetSpawnRotation));
                enemy.SpawnEnemy(this);

                break; 
            }
        }
    }
}
