using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/MoveUnitTowardsTargetAction")]
public class MoveUnitTowardsTargetAction : StateAction
{
    public override void Act(StateMachine controller)
    {
        if (controller.Owner is EnemyCharacter enemy)
        {
            Transform target;
            if (enemy.CurrentEnemyState == EnemyUnitState.GoingHome)
            {
                if (enemy.CurrentWaypointIndex < 0)
                {
                    target = Services.Get<IPathFinder>().EntrancePoint;
                }
                else if (enemy.CurrentWaypointIndex == 0 &&
                    Vector3.Distance(enemy.transform.position, Services.Get<IPathFinder>().Waypoints[0].transform.position) < 0.5f)
                {
                    target = Services.Get<IPathFinder>().EntrancePoint;
                }
                else target = Services.Get<IPathFinder>().Waypoints[enemy.CurrentWaypointIndex].transform;
            }
            else target = Services.Get<IPathFinder>().Waypoints[enemy.CurrentWaypointIndex].transform;

            // Calculate direction to the target
            Vector3 direction = target.transform.position - enemy.transform.position;

            // Ignore vertical movement
            direction.y = 0f;

            // Rotate towards the target direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            enemy.PreviousPosition = enemy.transform.position;

            // Move towards the target
            Vector3 newPosition = enemy.transform.position + enemy.CurrentMoveSpeed * Time.deltaTime * direction.normalized;
            Quaternion newRotation = Quaternion.Lerp(enemy.transform.rotation, targetRotation, enemy.EnemyData.TurnSpeed * Time.deltaTime);
            enemy.transform.SetPositionAndRotation(newPosition, newRotation);

            if (Vector3.Distance(enemy.transform.position, target.position) < 0.5f)
            {
                if (enemy.CurrentEnemyState == EnemyUnitState.AssaultingBase)
                {
                    if (enemy.CurrentWaypointIndex < Services.Get<IPathFinder>().Waypoints.Length - 1)
                        enemy.IncrementWaypointIndex();
                }
                else
                {
                    enemy.DecreaseWaypointIndex();
                }
            }
        }
     
    }
}