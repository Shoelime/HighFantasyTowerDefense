using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/UnitArrivedHome")]

public class UnitArrivedHomeAction : StateAction
{
    public override void Act(StateMachine controller)
    {
        if (controller.Owner is EnemyCharacter enemy)
        {
            enemy.EnemyArrivedHome();
        }
    }
}