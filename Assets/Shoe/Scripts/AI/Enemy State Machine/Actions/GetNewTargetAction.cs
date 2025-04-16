using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/FindTarget")]

public class FindTargetAction : StateAction
{
    public override void Act(StateMachine controller)
    {
        if (controller.Owner is TowerAI towerAI)
        {
            if (!controller.CheckIfCountDownElapsed(towerAI.TowerData.GetTowerSpecs(towerAI.TowerLevel).FireRate / 2))
                return;

            if (towerAI.TargetToShoot != null)
            {
                /// Find a new target to shoot if we can't see current target or current target is too far away
                /// 
                if (!MathUtils.HasLineOfSight(
                    towerAI.transform.position + towerAI.ProjectileStartPosition,
                    towerAI.TargetToShoot.transform, towerAI.TowerData.LineOfSightLayers,
                    towerAI.TowerData.GetTowerSpecs(towerAI.TowerLevel).SightRadius)
                    ||
                    Vector3.Distance(towerAI.transform.position, towerAI.TargetToShoot.transform.position) >
                    towerAI.TowerData.GetTowerSpecs(towerAI.TowerLevel).SightRadius)
                {
                    GetNearbyTargets(towerAI);
                }
            }
            else
            {
                /// Find a new target to shoot if there are non to be seen
                /// 
                GetNearbyTargets(towerAI);
            }

            controller.ResetStateTimer();
        }
    }

    void GetNearbyTargets(TowerAI towerAI)
    {
        Collider[] colliders = Physics.OverlapSphere(towerAI.transform.position, towerAI.TowerData.GetTowerSpecs(towerAI.TowerLevel).SightRadius, towerAI.TowerData.EnemyLayer);

        if (colliders.Length == 0)
        {
            towerAI.ClearTarget();
        }
        else SetClosestTarget(colliders, towerAI);
    }

    void SetClosestTarget(Collider[] colliders, TowerAI towerAI)
    {
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;
        towerAI.ClearEnemyList();

        for (int i = 0; i < colliders.Length; i++)
        {
            Debug.DrawRay(towerAI.transform.position + towerAI.ProjectileStartPosition,
                (colliders[i].transform.position + (Vector3.up * 0.5f)) - (towerAI.transform.position + towerAI.ProjectileStartPosition),
                Color.yellow,
                towerAI.TowerData.GetTowerSpecs(towerAI.TowerLevel).FireRate);

            /// Not adding inactive objects
            /// 
            if (!colliders[i].gameObject.activeInHierarchy)
            {
                continue;
            }

            /// No line of sight
            /// 
            if (!MathUtils.HasLineOfSight(
                towerAI.transform.position + towerAI.ProjectileStartPosition,
                colliders[i].transform, towerAI.TowerData.LineOfSightLayers,
                towerAI.TowerData.GetTowerSpecs(towerAI.TowerLevel).SightRadius))
            {
                continue;
            }

            towerAI.AddEnemyToList(colliders[i]);

            float distance = Vector3.Distance(towerAI.transform.position, colliders[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = colliders[i].transform;
            }
        }

        if (closestTarget != null)
        {
            towerAI.SetTarget(closestTarget.GetComponent<EnemyCharacter>());
        }
        else towerAI.ClearTarget();
    }
}
