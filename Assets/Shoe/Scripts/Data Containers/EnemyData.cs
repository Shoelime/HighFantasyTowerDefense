using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private EnemyType enemyType = default;
    [SerializeField] private int _hitPoints = 0;
    [SerializeField] private float moveSpeed = 0.0f;
    [SerializeField] private float turnSpeed = 0.0f;
    [SerializeField] private float gemCheckInterval = 0.4f;
    [SerializeField] private float gemPickupDistance = 0.4f;
    [SerializeField] private string description = null;
    [SerializeField] private int goldCarryCount = 0;
    [SerializeField] private GameObject worldPrefab;

    public EnemyType EnemyType { get { return enemyType; } }
    public int HitPoints { get { return _hitPoints; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float TurnSpeed { get { return turnSpeed; } }
    public float GemCheckInterval { get { return gemCheckInterval; } }
    public float GemPickupDistance { get { return gemPickupDistance; } }
    public string Description { get { return description; } }
    public int GoldCarryCount { get { return goldCarryCount; } }
    public GameObject WorldPrefab { get { return worldPrefab; } }
}

public enum EnemyType
{
    Footman,
    Hound,
    Crusader,
    BatteringRam,
    Priest,
    Captain
}