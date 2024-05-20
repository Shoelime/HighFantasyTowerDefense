using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeField] private Vector3 spawnRotation = Vector3.zero;
    public Vector3 GetSpawnRotation => spawnRotation;

    [SerializeField] private WaveComposition[] waveCompositions = null;
    [SerializeField] private float delayBetweenWaves;
    public WaveComposition[] WaveCompositions => waveCompositions;
    public float DelayBetweenWaves => delayBetweenWaves;
}

[System.Serializable]
public struct WaveComposition
{
    public EnemyType[] OrderOfEnemies;
    public int[] EnemyCountPerGroup;
    public float[] DelayBetweenGroups;
    public float[] DelayBetweenUnits;
}
