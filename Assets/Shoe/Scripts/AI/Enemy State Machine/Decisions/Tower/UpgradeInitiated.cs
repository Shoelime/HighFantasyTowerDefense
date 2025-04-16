using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsUpgradeInitiated")]
public class UpgradeInitiated : Decision
{
    public override bool Decide(StateMachine controller)
    {
        if (controller.Owner is TowerAI towerAI)
        {
            if (towerAI.UpgradeInitiated)
            {
                towerAI.SetConstructionParameters();
                return true;
            }

            return false;
        }
        return false;
    }
}
