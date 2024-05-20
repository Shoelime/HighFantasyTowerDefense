using System;
using System.Collections;
using UnityEngine;

public class TowerAI : StateController
{
    [Header("Child variables")]
    [SerializeField] private Vector3 projectileStartPosition;
    [SerializeField] private TowerData towerData;

    public Vector3 ProjectileStartPosition => projectileStartPosition;
    public TowerData TowerData => towerData;
    public EnemyCharacter TargetToShoot { get; private set; }
    public bool AttackCooldownActive { get; private set; }
    public override EnemyCharacter EnemyAiController => null;
    public override TowerAI TowerAiController => GetComponent<TowerAI>();

    public void BuildTower()
    {
        foreach (var mover in GetComponentsInChildren<MoveObjectToPosition>())
        {
            mover.StartMovement();
        }

        SetupAI(true);
        Invoke(nameof(SetTowerStartingTimers), towerData.ConstructDuration);
    }

    public void ClearTarget()
    {
        TargetToShoot = null;
    }

    public void CooldownTrigger()
    {
        AttackCooldownActive = true;
        StartCoroutine(CooldownCoroutine());
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, towerData.SightRadius);
    }
}
