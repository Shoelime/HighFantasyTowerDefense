using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsUnitNearBaseDecision")]
public class IsUnitNearBase : Decision
{
    public override bool Decide(StateController controller)
    {
        if (Vector3.Distance(controller.transform.position, Services.Get<IPathFinder>().Waypoints[^1].transform.position) < 0.5f)
        {
            controller.EnemyAiController.EnemyReachedBase();
            return true;
        }
        return false;
    }
}