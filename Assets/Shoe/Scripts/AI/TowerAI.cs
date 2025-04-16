using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : PooledMonoBehaviour
{
    [Header("Child variables")]
    [SerializeField] private Vector3 projectileStartPosition;
    [SerializeField] private TowerData towerData;
    [SerializeField] private Transform basePieces;
    [SerializeField] private Transform[] upgradePieces;
    [SerializeField] private State remainState;
    [SerializeField] private State startState;
    public Vector3 ProjectileStartPosition => projectileStartPosition;
    public TowerData TowerData => towerData;
    public EnemyCharacter TargetToShoot { get; private set; }
    public List<Collider> AllTargets { get; private set; }
    public bool AttackCooldownActive { get; private set; }
    public ITowerAction TowerAction { get; private set; }
    public bool UpgradeInitiated { get; private set; }
    public int TowerLevel { get; private set; }
    public bool IsMaxLevel() { return TowerLevel == 1; }

    private TowerPlate towerPlate;
    public StateMachine stateMachine { get; private set; }
    public State StartState => startState;

    private void OnEnable()
    {
        TowerLevel = -1;

        if (stateMachine == null)
        {
            stateMachine = new StateMachine
            {
                Owner = this,
                RemainState = remainState
            };
        }
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void BuildTower(TowerPlate towerPlate)
    {
        foreach (var mover in basePieces.GetComponentsInChildren<MoveObjectToPosition>())
        {
            mover.StartMovement();
        }

        TowerLevel++;

        SetAttackType();

        stateMachine.Init(StartState);
        Invoke(nameof(SetTowerStartingTimers), towerData.ConstructDuration);

        AllTargets = new List<Collider>();

        this.towerPlate = towerPlate;
    }

    public void UpgradeTower()
    {
        UpgradeInitiated = true;

        CancelInvoke(nameof(SetTowerStartingTimers));
        Invoke(nameof(SetTowerStartingTimers), towerData.ConstructDuration);

        foreach (var mover in upgradePieces[TowerLevel].GetComponentsInChildren<MoveObjectToPosition>())
        {
            mover.StartMovement();
        }

        TowerLevel++;
    }

    void SetAttackType()
    {
        TowerAction = towerData.TowerType switch
        {
            TowerType.Earth => new ShootProjectileAction(),
            TowerType.Water => new AreaAttackAction(),
            TowerType.Air => new InstantAttackAction(),
            TowerType.Fire => new ShootProjectileAction(),
            _ => default,
        };
    }

    void SetTowerStartingTimers()
    {
        stateMachine.SetTimer(5);
    }

    public void ButtonPressed(Vector3 clickPosition, GameObject clickedObject)
    {
        var isInHierarchy = MathUtils.IsInParentHierarchy(clickedObject, transform);

        if (isInHierarchy == null)
        {
            if (towerPlate.plateSelected)
                towerPlate.SelectPlate(false);
        }
        else if (isInHierarchy == true)
        {
            // The clicked object is in the tower's hierarchy
            if (towerPlate.plateSelected)
                towerPlate.SelectPlate(false);
            else
                towerPlate.SelectPlate(true);
        }
        else
        {
            if (towerPlate.plateSelected)
                towerPlate.SelectPlate(false);
        }
    }

    public void ClearTarget()
    {
        TargetToShoot = null;
    }

    public void AddEnemyToList(Collider enemy)
    {
        AllTargets.Add(enemy);
    }

    public void ClearEnemyList()
    {
        AllTargets.Clear();
    }

    public void CooldownTrigger()
    {
        if (!AttackCooldownActive)
        {
            AttackCooldownActive = true;
            StartCoroutine(CooldownCoroutine());
        }
    }

    public IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(towerData.GetTowerSpecs(TowerLevel).FireRate);
        AttackCooldownActive = false;
    }

    public void SetTarget(EnemyCharacter transform)
    {
        TargetToShoot = transform;
    }

    public void SetConstructionParameters()
    {
        UpgradeInitiated = false;
    }

    public DamageData GetTowerDamageData()
    {
        return TowerData.GetTowerSpecs(TowerLevel).DamageData;
    }

    public TowerUpgradableVariables GetTowerSpecs()
    {
        return TowerData.GetTowerSpecs(TowerLevel);
    }

    private new void OnDisable()
    {
        Services.Get<IInputManager>().OnLeftMouseButton -= ButtonPressed;
        base.OnDisable();
    }
}
