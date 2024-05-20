using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/FindTarget")]

public class FindTargetAction : StateAction
{
    public override void Act(StateController controller)
    {
        if (!controller.CheckIfCountDownElapsed(controller.TowerAiController.TowerData.FireRate / 2))
            return;

        if (controller.TowerAiController.TargetToShoot != null)
        {
            /// Find a new target to shoot if we can't see current target or current target is too far away
            /// 
            if (!MathUtils.HasLineOfSight(
                controller.transform.position + controller.TowerAiController.ProjectileStartPosition, controller.TowerAiController.TargetToShoot.transform, controller.TowerAiController.TowerData.LineOfSightLayers, controller.TowerAiController.TowerData.SightRadius) ||
                Vector3.Distance(controller.transform.position, controller.TowerAiController.TargetToShoot.transform.position) > controller.TowerAiController.TowerData.SightRadius)
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
        Collider[] colliders = Physics.OverlapSphere(controller.transform.position, controller.TowerAiController.TowerData.SightRadius, controller.TowerAiController.TowerData.EnemyLayer);

        if (colliders.Length == 0)
        {
            //Debug.Log("no targets nearby");
            controller.TowerAiController.ClearTarget();
        }
        else SetClosestTarget(controller, colliders);
    }

    void SetClosestTarget(StateController controller, Collider[] colliders)
    {
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < colliders.Length; i++)
        {
            Debug.DrawRay(controller.transform.position + controller.TowerAiController.ProjectileStartPosition, 
                (colliders[i].transform.position + (Vector3.up * 0.5f)) - (controller.transform.position + controller.TowerAiController.ProjectileStartPosition), 
                Color.yellow,
                controller.TowerAiController.TowerData.FireRate);

            if (!colliders[i].gameObject.activeInHierarchy)
            {
                continue;
            }

            if (!MathUtils.HasLineOfSight(controller.transform.position + controller.TowerAiController.ProjectileStartPosition, colliders[i].transform, controller.TowerAiController.TowerData.LineOfSightLayers, controller.TowerAiController.TowerData.SightRadius))
            {
                //Debug.Log("no line of sight");
                continue;
            }

            float distance = Vector3.Distance(controller.transform.position, colliders[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = colliders[i].transform;
            }
        }

        if (closestTarget != null)
        {
            //Debug.Log("set target");
            controller.TowerAiController.SetTarget(closestTarget.GetComponent<EnemyCharacter>());
        }
        else controller.TowerAiController.ClearTarget();
        //else Debug.Log("empty");
    }
}
