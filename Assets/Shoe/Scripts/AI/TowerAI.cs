using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : StateController
{
    [Header("Child variables")]
    [SerializeField] private Vector3 projectileStartPosition;
    [SerializeField] private TowerData towerData;
    [SerializeField] private Transform basePieces;
    [SerializeField] private Transform[] upgradePieces;
    public Vector3 ProjectileStartPosition => projectileStartPosition;
    public TowerData TowerData => towerData;
    public EnemyCharacter TargetToShoot { get; private set; }
    public List<Collider> AllTargets { get; private set; }
    public bool AttackCooldownActive { get; private set; }
    public ITowerAction TowerAction { get; private set; }
    public bool UpgradeInitiated { get; private set; }
    public int TowerLevel { get; private set; }
    public bool IsMaxLevel() { return TowerLevel == 1; }

    private void OnEnable()
    {
        TowerLevel = -1;

        if (TowerAiController == null)
            TowerAiController = this;
    }

    public void BuildTower()
    {
        foreach (var mover in basePieces.GetComponentsInChildren<MoveObjectToPosition>())
        {
            mover.StartMovement();
        }

        TowerLevel++;

        SetAttackType();
        SetupAI(true);
        Invoke(nameof(SetTowerStartingTimers), towerData.ConstructDuration);

        AllTargets = new List<Collider>();
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
}
