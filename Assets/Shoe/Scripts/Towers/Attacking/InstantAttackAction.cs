using UnityEngine;

public class InstantAttackAction : ITowerAction
{
    public void Execute(TowerAI controller)
    {
        if (controller.TargetToShoot == null)
        {
            return;
        }

        var projectile = controller.TowerData.ProjectileToShoot.Get<Projectile>(
            controller.transform.position + controller.ProjectileStartPosition,
            Quaternion.identity);

        Vector3 startPosition = controller.transform.TransformPoint(controller.ProjectileStartPosition);
        Vector3 endPosition = controller.TargetToShoot.transform.position;

        projectile.LineRenderer.enabled = true;
        projectile.LineRenderer.SetPosition(0, startPosition);
        projectile.LineRenderer.SetPosition(1, endPosition);

        projectile.SetDamageData(controller.GetTowerDamageData());
        projectile.DealDamage(controller.TargetToShoot);

        controller.CooldownTrigger();
    }
}
