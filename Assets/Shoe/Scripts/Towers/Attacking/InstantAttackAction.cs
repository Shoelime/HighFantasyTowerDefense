using UnityEngine;

public class InstantAttackAction : ITowerAction
{
    public void Execute(StateController controller)
    {
        if (controller.TowerAiController.TargetToShoot == null)
        {
            return;
        }

        var projectile = controller.TowerAiController.TowerData.ProjectileToShoot.Get<Projectile>(
            controller.TowerAiController.transform.position + controller.TowerAiController.ProjectileStartPosition, 
            Quaternion.identity);

        Vector3 startPosition = controller.TowerAiController.transform.TransformPoint(controller.TowerAiController.ProjectileStartPosition);
        Vector3 endPosition = controller.TowerAiController.TargetToShoot.transform.position;

        projectile.LineRenderer.enabled = true;
        projectile.LineRenderer.SetPosition(0, startPosition);
        projectile.LineRenderer.SetPosition(1, endPosition);

        projectile.SetDamageData(controller.TowerAiController.TowerData.DamageData);
        projectile.DealDamage(controller.TowerAiController.TargetToShoot);

        controller.TowerAiController.CooldownTrigger();  
    }
}
