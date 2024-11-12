using UnityEngine;

public class DeathParticleSpawner : MonoBehaviour
{
    [SerializeField] private DeathParticle footmanDeathParticles;
    [SerializeField] private DeathParticle houndDeathParticles;
    [SerializeField] private DeathParticle crusaderDeathParticles;
    [SerializeField] private Vector3 spawnRotation;

    [Header("For debugging purposes")]
    [SerializeField] private EnemyData[] enemyData;

    private void Start()
    {
        EnemyCharacter.EnemyDied += SpawnDeathParticles;
    }
#if UNITY_EDITOR
    /// <summary>
    /// For fast testing
    /// </summary>
    [Button]
    void SpawnRandomParticle()
    {
        int randomData = Random.Range(0, 3);
        SpawnDeathParticles(enemyData[randomData], new Vector3(-5 + randomData, 0, 6 + randomData));
    }
#endif
    /// <summary>
    /// Spawn death particles based on enemy type, to their death location
    /// </summary>
    /// <param name="enemyData"></param>
    /// <param name="pos"></param>
    private void SpawnDeathParticles(EnemyData enemyData, Vector3 pos)
    {
        switch (enemyData.EnemyType)
        {
            case EnemyType.Footman:
                footmanDeathParticles.Get<DeathParticle>(pos, Quaternion.Euler(spawnRotation));
                break;
            case EnemyType.Hound:
                houndDeathParticles.Get<DeathParticle>(pos, Quaternion.Euler(spawnRotation));
                break;
            case EnemyType.Crusader:
                crusaderDeathParticles.Get<DeathParticle>(pos, Quaternion.Euler(spawnRotation));
                break;
        }
    }

    private void OnDisable()
    {
        EnemyCharacter.EnemyDied -= SpawnDeathParticles;
    }
}
