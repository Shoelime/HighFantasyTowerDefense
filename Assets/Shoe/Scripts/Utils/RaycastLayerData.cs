using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/RaycastLayers")]
public class RaycastLayerData : ScriptableObject
{
    [SerializeField] private LayerMask layersToDetect;
    [SerializeField] private LayerMask towerPlateLayer;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private LayerMask enemyLayer;

    public LayerMask LayersToDetect => layersToDetect;
    public LayerMask TowerPlateLayer => towerPlateLayer;
    public LayerMask TowerLayer => towerLayer;
    public LayerMask EnemyLayer => enemyLayer;
}