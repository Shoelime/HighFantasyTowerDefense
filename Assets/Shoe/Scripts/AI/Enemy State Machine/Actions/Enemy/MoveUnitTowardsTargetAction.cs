using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/MoveUnitTowardsTargetAction")]
public class MoveUnitTowardsTargetAction : StateAction
{
    public override void Act(StateController controller)
    {
        Transform target;
        if (controller.EnemyAiController.CurrentEnemyState == EnemyUnitState.GoingHome)
        {
            if (controller.EnemyAiController.CurrentWaypointIndex < 0)
            {
                target = Services.Get<IPathFinder>().EntrancePoint;
            }
            else if (controller.EnemyAiController.CurrentWaypointIndex == 0 &&
                Vector3.Distance(controller.transform.position, Services.Get<IPathFinder>().Waypoints[0].transform.position) < 0.5f)
            {
                target = Services.Get<IPathFinder>().EntrancePoint;
            }
            else target = Services.Get<IPathFinder>().Waypoints[controller.EnemyAiController.CurrentWaypointIndex].transform;
        }
        else target = Services.Get<IPathFinder>().Waypoints[controller.EnemyAiController.CurrentWaypointIndex].transform;

        // Calculate direction to the target
        Vector3 direction = target.transform.position - controller.transform.position;

        // Ignore vertical movement
        direction.y = 0f;

        // Rotate towards the target direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        controller.EnemyAiController.PreviousPosition = controller.transform.position;

        // Move towards the target
        Vector3 newPosition = controller.transform.position + controller.EnemyAiController.EnemyData.MoveSpeed * Time.deltaTime * direction.normalized;
        Quaternion newRotation = Quaternion.Lerp(controller.transform.rotation, targetRotation, controller.EnemyAiController.EnemyData.TurnSpeed * Time.deltaTime);
        controller.transform.SetPositionAndRotation(newPosition, newRotation);

        if (Vector3.Distance(controller.transform.position, target.position) < 0.5f)
        {
            if (controller.EnemyAiController.CurrentEnemyState == EnemyUnitState.AssaultingBase)
            {
                if (controller.EnemyAiController.CurrentWaypointIndex<Services.Get<IPathFinder>().Waypoints.Length - 1)
                    controller.EnemyAiController.IncrementWaypointIndex();
            }
            else
            {
                controller.EnemyAiController.DecreaseWaypointIndex();
            }
        }
    }
}