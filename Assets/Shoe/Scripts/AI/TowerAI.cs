using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : StateController
{
    [Header("Child variables")]
    [SerializeField] private Vector3 projectileStartPosition;
    [SerializeField] private TowerData towerData;

    public Vector3 ProjectileStartPosition => projectileStartPosition;
    public TowerData TowerData => towerData;
    public EnemyCharacter TargetToShoot { get; private set; }
    public List<Collider> AllTargets { get; private set; }
    public bool AttackCooldownActive { get; private set; }
    public ITowerAction TowerAction { get; private set; }

    private void OnEnable()
    {
        if (TowerAiController == null)
            TowerAiController = this;
    }

    public void BuildTower()
    {
        foreach (var mover in GetComponentsInChildren<MoveObjectToPosition>())
        {
            mover.StartMovement();
        }

        SetAttackType();
        SetupAI(true);
        Invoke(nameof(SetTowerStartingTimers), towerData.ConstructDuration);

        AllTargets = new List<Collider>();
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
        yield return new WaitForSeconds(towerData.FireRate);
        AttackCooldownActive = false;
    }

    public void SetTarget(EnemyCharacter transform)
    {
        TargetToShoot = transform;
    }
}
