using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsUnitNearHomeDecision")]
public class IsUnitNearHome : Decision
{
    public override bool Decide(StateController controller)
    {
        return Vector3.Distance(controller.transform.position, Services.Get<IPathFinder>().EntrancePoint.position) < 0.3f;
    }
}
