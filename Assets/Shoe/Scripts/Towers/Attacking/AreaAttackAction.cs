using UnityEngine;

public class AreaAttackAction : ITowerAction
{
    public void Execute(StateController controller)
    {
        if (controller.TowerAiController.TargetToShoot == null)
        {
            return;
        }

        var projectile = controller.TowerAiController.TowerData.ProjectileToShoot.Get<Projectile>(
            controller.TowerAiController.transform.position,
            Quaternion.identity);

        projectile.SetDamageData(controller.TowerAiController.GetTowerDamageData());
        projectile.DealDamage(null);

        controller.TowerAiController.CooldownTrigger();
    }
}
