using System;
using UnityEngine;

public class EnemyCharacter : StateController
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private GameObject visualObject;
    private HealthBar healthBar;
    public float CurrentMoveSpeed { get; private set; }
    public float RelativeSpeedMultiplier { get; private set; }
    public EnemyData EnemyData => enemyData;
    public GameObject GemBeingCarried { get; private set; }
    public Health HealthComponent { get; private set; }
    public WaveManager WaveManager { get; private set; }
    public EnemyUnitState CurrentEnemyState { get; private set; }
    public Vector3 PreviousPosition { get; internal set; }
    public int CurrentWaypointIndex { get; private set; }
    public override TowerAI TowerAiController => null;
    public override EnemyCharacter EnemyAiController => GetComponent<EnemyCharacter>();

    public static event Action<EnemyData> EnemyDied;
    public static event Action<EnemyCharacter> EnemySnatchedGem;
    public static event Action<EnemyCharacter> EnemyArrivedToBase;
    public static event Action<EnemyCharacter> EnemyArrivedHomeEvent;

    void OnEnable()
    {
        if (HealthComponent == null)
            HealthComponent = GetComponent<Health>();

        HealthComponent.SetStartingHealth(enemyData.HitPoints);
        HealthComponent.HealthReachedZero += Death;
        HealthComponent.HealthReduced += HealthReduced;
        OnReturnToPool += RemoveEvents;

        CurrentMoveSpeed = EnemyData.MoveSpeed;

        SetEnemyState(EnemyUnitState.AssaultingBase);

        CurrentWaypointIndex = 0;
    }

    private void RemoveEvents(PooledMonoBehaviour obj)
    {
        HealthComponent.HealthReachedZero -= Death;
        HealthComponent.HealthReduced -= HealthReduced;
        OnReturnToPool -= RemoveEvents;
    }

    public void AssignHealthBar(HealthBar healthBar)
    {
        this.healthBar = healthBar;
    }

    private void HealthReduced(int obj)
    {
        healthBar.UpdateHealthBar(obj, enemyData.HitPoints);
    }

    private void FixedUpdate()
    {
        RelativeSpeedMultiplier = CurrentMoveSpeed / EnemyData.MoveSpeed;
    }

    public void SpawnEnemy(WaveManager waveManager)
    {
        WaveManager = waveManager;
        SetupAI(true);
    }

    public void PickupGem(GameObject gemObject)
    {
        GemBeingCarried = gemObject;
        SetEnemyState(EnemyUnitState.GoingHome);
        EnemySnatchedGem?.Invoke(this);
    }

    void DropGem()
    {
        if (GemBeingCarried == null)
            return;

        GemBeingCarried.transform.position = transform.position + Vector3.up;
        GemBeingCarried.SetActive(true);
        GemBeingCarried.transform.SetParent(null);
    }

    public void EnemyReachedBase()
    {
        SetEnemyState(EnemyUnitState.GoingHome);
        EnemyArrivedToBase?.Invoke(this);
    }

    public void SetEnemyState(EnemyUnitState enemyState)
    {
        CurrentEnemyState = enemyState;
    }

    public void EnemyArrivedHome()
    {
        EnemyArrivedHomeEvent?.Invoke(this);
        ResetStateMachine();
        ReturnToPool();
    }

    public void Death()
    {
        DropGem();

        EnemyDied?.Invoke(enemyData);

        ResetStateMachine();
        ReturnToPool();
    }

    public void IncrementWaypointIndex()
    {
        CurrentWaypointIndex++;
    }

    public void DecreaseWaypointIndex()
    {
        CurrentWaypointIndex--;
    }
}

public enum EnemyUnitState
{
    None,
    AssaultingBase,
    GoingHome,
    Dead
}