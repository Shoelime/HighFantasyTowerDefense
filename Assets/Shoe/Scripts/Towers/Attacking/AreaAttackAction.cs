public class AreaAttackAction : ITowerAction
{
    public void Execute(StateController controller)
    {
        foreach (var enemy in controller.TowerAiController.AllTargets)
        {
            enemy.GetComponent<IHealth>().TakeDamage(controller.TowerAiController.TowerData.DamageData);
        }
    }
}
