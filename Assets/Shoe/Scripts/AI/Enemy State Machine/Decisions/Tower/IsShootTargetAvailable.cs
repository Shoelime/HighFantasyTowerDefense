using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsShootTargetAvailable")]
public class IsShootTargetAvailable : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.TowerAiController.TargetToShoot != null;
    }
}
