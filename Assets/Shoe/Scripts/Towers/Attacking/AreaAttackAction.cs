using UnityEngine;

public class AreaAttackAction : ITowerAction
{
    public void Execute(StateController controller)
    {
        var projectile = controller.TowerAiController.TowerData.ProjectileToShoot.Get<Projectile>(
            controller.TowerAiController.transform.position,
            Quaternion.identity);

        projectile.SetDamageData(controller.TowerAiController.TowerData.DamageData);
        projectile.DealDamage(null);

        controller.TowerAiController.CooldownTrigger();
    }
}
