using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/IsTowerConstructedDecision")]
public class IsTowerConstructed : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.CheckIfCountDownElapsed(controller.TowerAiController.TowerData.ConstructDuration);
    }
}
