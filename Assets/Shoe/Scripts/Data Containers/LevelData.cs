using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int gemCount;
    [SerializeField] private int startingGold;
    [SerializeField] private GameObject gemContainerPrefab;
    [SerializeField] private WaveData waveData;

    public int StartingGold => startingGold;
    public int GemCount => gemCount;
    public GameObject GemContainerPrefab => gemContainerPrefab;
    public WaveData WaveData => waveData;
}
