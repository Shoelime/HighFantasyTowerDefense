using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsShootTargetAvailable")]
public class IsShootTargetAvailable : Decision
{
    public override bool Decide(StateMachine controller)
    {
        if (controller.Owner is TowerAI towerAI)
        {
            return towerAI.TargetToShoot != null;
        }

        return false;
    }
}
