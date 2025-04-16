using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/AttackAction")]

public class TowerAttackAction : StateAction
{
    public override void Act(StateMachine controller)
    {
        if (controller.Owner is TowerAI towerAI)
        {
            if (towerAI.AttackCooldownActive)
                return;

            towerAI.TowerAction.Execute(towerAI);
        }
    }
}
