using System;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : IGemManager
{
    private GameObject[] gemPrefabs;
    public int GemCountAtBase { get; private set; }
    public List<GameObject> SnatchedGems { get; private set; }
    public List<GameObject> AvailableGems { get; private set; }

    public static event Action AllGemsLost;

    private GameObject[] spawnedGems;
    private GemContainer container;

    public void Initialize()
    {
        gemPrefabs = Resources.LoadAll<GameObject>("Gems");
        SpawnGems();

        EnemyCharacter.EnemyArrivedToBase += (enemyData) => EnemyReachedBase(enemyData);
        EnemyCharacter.EnemyArrivedHomeEvent += (enemyData) => GemStolen(enemyData.GemBeingCarried);
    }

    /// <summary>
    /// Spawn the gems at startup
    /// </summary>
    private void SpawnGems()
    {
        // Get gem count
        GemCountAtBase = Services.Get<IGameManager>().GetLevelData.GemCount;

        // Instantiate gem container at the last waypoint position
        container = GameObject.Instantiate(
            Services.Get<IGameManager>().GetLevelData.GemContainerPrefab,
             Services.Get<IPathFinder>().Waypoints[^1].transform.position,
            Quaternion.identity).GetComponent<GemContainer>();

        // Initialize the container
        container.Initialize();

        SnatchedGems = new List<GameObject>();
        AvailableGems = new List<GameObject>();
        spawnedGems = new GameObject[GemCountAtBase];

        // Spawn the gems
        for (int i = 0; i < GemCountAtBase; i++)
        {
            spawnedGems[i] = GameObject.Instantiate(
                gemPrefabs[i], 
                container.GemHolders[i].position, 
                Quaternion.identity, 
                container.GemHolders[i]);

            AvailableGems.Add(spawnedGems[i]);
        }
    }

    /// <summary>
    /// Remove gem when enemy reaches base if there are any left
    /// </summary>
    /// <param name="enemyCharacter"></param>
    public void EnemyReachedBase(EnemyCharacter enemyCharacter)
    {
        if (GemCountAtBase > 0)
        {
            EnemySnatchesGem(enemyCharacter.transform.position, enemyCharacter);
            GemCountAtBase--;
        }
    }

    /// <summary>
    /// Disable the closest gem object and assign it to the enemy
    /// </summary>
    /// <param name="fromPos"></param>
    /// <param name="enemyCharacter"></param>
    public void EnemySnatchesGem(Vector3 fromPos, EnemyCharacter enemyCharacter)
    {
        var closestGem = MathUtils.FindClosestGameObject(fromPos, spawnedGems);
        closestGem.SetActive(false);

        SnatchedGems.Add(closestGem);
        enemyCharacter.PickupGem(closestGem);
    }

    /// <summary>
    /// Trigger events that are related to enemy reaching home with gem
    /// </summary>
    /// <param name="gemObject"></param>
    public void GemStolen(GameObject gemObject)
    {
        if (gemObject == null)
            return;

        Debug.Log("enemy took gem home");
        AvailableGems.Remove(gemObject);

        if (AvailableGems.Count == 0)
            AllGemsLost?.Invoke();
    }
}