using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsGemNearby")]
public class IsGemNearby : Decision
{
    public override bool Decide(StateMachine controller)
    {
        if (controller.Owner is EnemyCharacter enemy)
        {
            if (enemy.GemBeingCarried != null)
                return false;

            if (!controller.CheckIfCountDownElapsed(enemy.EnemyData.GemCheckInterval))
                return false;
            else
            {
                controller.ResetStateTimer();
                GameObject closestGem = MathUtils.FindClosestGameObject(enemy.transform.position, Services.Get<IGemManager>().AvailableGems.ToArray());

                if (closestGem != null)
                {
                    float dist = Vector3.Distance(enemy.transform.position, closestGem.transform.position);
                    if (dist <= enemy.EnemyData.GemPickupDistance)
                    {
                        if (enemy.CurrentEnemyState == EnemyUnitState.AssaultingBase)
                            enemy.DecreaseWaypointIndex();

                        Services.Get<IGemManager>().EnemySnatchesGem(enemy.transform.position, enemy);

                        return true;
                    }
                }
            }

            return false;
        }
        return false;
    }
}
