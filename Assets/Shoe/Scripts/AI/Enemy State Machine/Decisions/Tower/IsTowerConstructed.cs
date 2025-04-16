using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsTowerConstructedDecision")]
public class IsTowerConstructed : Decision
{
    public override bool Decide(StateMachine controller)
    {
        if (controller.Owner is TowerAI towerAI)
        {
            return controller.CheckIfCountDownElapsed(towerAI.TowerData.ConstructDuration);
        }
        return false;
    }
}
