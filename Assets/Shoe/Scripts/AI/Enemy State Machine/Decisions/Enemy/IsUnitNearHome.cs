using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsUnitNearHomeDecision")]
public class IsUnitNearHome : Decision
{
    public override bool Decide(StateMachine controller)
    {
        if (controller.Owner is EnemyCharacter enemy)
        {
            return Vector3.Distance(
                enemy.transform.position,
                enemy.PathFinder.EntrancePoint.position
            ) < 0.3f;
        }

        return false;
    }
}
