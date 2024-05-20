using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/UnitArrivedHome")]

public class UnitArrivedHomeAction : StateAction
{
    public override void Act(StateController controller)
    {
        controller.EnemyAiController.EnemyArrivedHome();
    }
}