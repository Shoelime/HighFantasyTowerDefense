public class InstantAttackAction : ITowerAction
{
    public void Execute(StateController controller)
    {
        if (controller.TowerAiController.TargetToShoot == null)
        {
            return;
        }

        //controller.TowerAiController.TargetToShoot.TakeDamage(controller.TowerAiController.TowerData.ZapDamage);
    }
}
