using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsUpgradeInitiated")]
public class UpgradeInitiated : Decision
{
    public override bool Decide(StateController controller)
    {
        if (controller.TowerAiController.UpgradeInitiated)
        {
            controller.TowerAiController.SetConstructionParameters();
            return true;
        }

        return false;
    }
}
