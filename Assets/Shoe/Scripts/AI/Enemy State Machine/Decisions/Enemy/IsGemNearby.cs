using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsGemNearby")]
public class IsGemNearby : Decision
{
    public override bool Decide(StateController controller)
    {
        if (controller.EnemyAiController.GemBeingCarried != null)
            return false;

        if (!controller.CheckIfCountDownElapsed(controller.EnemyAiController.EnemyData.GemCheckInterval))
            return false;
        else
        {
            controller.ResetStateTimer();
            GameObject closestGem = MathUtils.FindClosestGameObject(controller.transform.position, Services.Get<IGemManager>().AvailableGems.ToArray());

            if (closestGem != null)
            {
                float dist = Vector3.Distance(controller.transform.position, closestGem.transform.position);
                if (dist <= controller.EnemyAiController.EnemyData.GemPickupDistance)
                {
                    if (controller.EnemyAiController.CurrentEnemyState == EnemyUnitState.AssaultingBase)
                        controller.EnemyAiController.DecreaseWaypointIndex();

                    Services.Get<IGemManager>().EnemySnatchesGem(controller.transform.position, controller.EnemyAiController);

                    return true;
                }
            }
        }

        return false;
    }
}
