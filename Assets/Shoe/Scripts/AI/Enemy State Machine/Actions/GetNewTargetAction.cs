using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/FindTarget")]

public class FindTargetAction : StateAction
{
    public override void Act(StateController controller)
    {
        if (!controller.CheckIfCountDownElapsed(controller.TowerAiController.TowerData.GetTowerSpecs(controller.TowerAiController.TowerLevel).FireRate / 2))
            return;

        if (controller.TowerAiController.TargetToShoot != null)
        {
            /// Find a new target to shoot if we can't see current target or current target is too far away
            /// 
            if (!MathUtils.HasLineOfSight(
                controller.transform.position + controller.TowerAiController.ProjectileStartPosition,
                controller.TowerAiController.TargetToShoot.transform, controller.TowerAiController.TowerData.LineOfSightLayers,
                controller.TowerAiController.TowerData.GetTowerSpecs(controller.TowerAiController.TowerLevel).SightRadius)
                ||
                Vector3.Distance(controller.transform.position, controller.TowerAiController.TargetToShoot.transform.position) >
                controller.TowerAiController.TowerData.GetTowerSpecs(controller.TowerAiController.TowerLevel).SightRadius)
            {
                GetNearbyTargets(controller);
            }
        }
        else
        {
            /// Find a new target to shoot if there are non to be seen
            /// 
            GetNearbyTargets(controller);
        }

        controller.ResetStateTimer();
    }

    void GetNearbyTargets(StateController controller)
    {
        Collider[] colliders = Physics.OverlapSphere(controller.transform.position, controller.TowerAiController.TowerData.GetTowerSpecs(controller.TowerAiController.TowerLevel).SightRadius, controller.TowerAiController.TowerData.EnemyLayer);

        if (colliders.Length == 0)
        {
            controller.TowerAiController.ClearTarget();
        }
        else SetClosestTarget(controller, colliders);
    }

    void SetClosestTarget(StateController controller, Collider[] colliders)
    {
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;
        controller.TowerAiController.ClearEnemyList();

        for (int i = 0; i < colliders.Length; i++)
        {
            Debug.DrawRay(controller.transform.position + controller.TowerAiController.ProjectileStartPosition,
                (colliders[i].transform.position + (Vector3.up * 0.5f)) - (controller.transform.position + controller.TowerAiController.ProjectileStartPosition),
                Color.yellow,
                controller.TowerAiController.TowerData.GetTowerSpecs(controller.TowerAiController.TowerLevel).FireRate);

            /// Not adding inactive objects
            /// 
            if (!colliders[i].gameObject.activeInHierarchy)
            {
                continue;
            }

            /// No line of sight
            /// 
            if (!MathUtils.HasLineOfSight(
                controller.transform.position + controller.TowerAiController.ProjectileStartPosition,
                colliders[i].transform, controller.TowerAiController.TowerData.LineOfSightLayers,
                controller.TowerAiController.TowerData.GetTowerSpecs(controller.TowerAiController.TowerLevel).SightRadius))
            {
                continue;
            }

            controller.TowerAiController.AddEnemyToList(colliders[i]);

            float distance = Vector3.Distance(controller.transform.position, colliders[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = colliders[i].transform;
            }
        }

        if (closestTarget != null)
        {
            controller.TowerAiController.SetTarget(closestTarget.GetComponent<EnemyCharacter>());
        }
        else controller.TowerAiController.ClearTarget();
    }
}
