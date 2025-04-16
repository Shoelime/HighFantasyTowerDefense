using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsUnitNearBaseDecision")]
public class IsUnitNearBase : Decision
{
    public override bool Decide(StateMachine controller)
    {
        if (controller.Owner is EnemyCharacter enemy)
        {
            if (Vector3.Distance(enemy.transform.position, Services.Get<IPathFinder>().Waypoints[^1].transform.position) < 0.5f)
            {
                enemy.EnemyReachedBase();
                return true;
            }
            return false;
        }
        return false;
    }
}