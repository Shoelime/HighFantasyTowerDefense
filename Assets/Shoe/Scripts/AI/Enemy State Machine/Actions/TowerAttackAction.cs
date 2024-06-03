using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/AttackAction")]

public class TowerAttackAction : StateAction
{
    public override void Act(StateController controller)
    {
        if (controller.TowerAiController.AttackCooldownActive)
            return;

        controller.TowerAiController.TowerAction.Execute(controller);
    }
}
