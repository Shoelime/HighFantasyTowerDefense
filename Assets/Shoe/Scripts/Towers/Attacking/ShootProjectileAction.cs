using UnityEngine;

public class ShootProjectileAction : ITowerAction
{
    public void Execute(TowerAI controller)
    {
        if (controller.TargetToShoot == null)
        {
            return;
        }

        Vector3 previousPosition = controller.TargetToShoot.PreviousPosition;
        Vector3 currentPosition = controller.TargetToShoot.transform.position;

        var projectile = controller.TowerData.ProjectileToShoot.Get<Projectile>(
            controller.transform.position + controller.ProjectileStartPosition,
            Quaternion.identity);

        Vector3 interceptPoint = MathUtils.CalculateInterceptPoint(
            projectile.transform.position,
            currentPosition,
            previousPosition,
            controller.GetTowerSpecs().ProjectileSpeed,
            out float duration);

        projectile.SetDamageData(controller.GetTowerDamageData());
        projectile.CallCoroutine(projectile.transform.position, interceptPoint, duration, controller.TargetToShoot);

        controller.CooldownTrigger();
    }
}
