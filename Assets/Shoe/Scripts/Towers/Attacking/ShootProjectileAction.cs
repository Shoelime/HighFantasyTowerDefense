using UnityEngine;

public class ShootProjectileAction : ITowerAction
{
    public void Execute(StateController controller)
    {
        if (controller.TowerAiController.TargetToShoot == null)
        {
            return;
        }

        Vector3 previousPosition = controller.TowerAiController.TargetToShoot.PreviousPosition;
        Vector3 currentPosition = controller.TowerAiController.TargetToShoot.transform.position;

        var projectile = controller.TowerAiController.TowerData.ProjectileToShoot.Get<Projectile>(
            controller.TowerAiController.transform.position + controller.TowerAiController.ProjectileStartPosition, 
            Quaternion.identity);

        Vector3 interceptPoint = MathUtils.CalculateInterceptPoint(
            projectile.transform.position,
            currentPosition,
            previousPosition,
            controller.TowerAiController.TowerData.ProjectileSpeed,
            out float duration);

        projectile.SetDamageData(controller.TowerAiController.TowerData.DamageData);
        projectile.CallCoroutine(projectile.transform.position, interceptPoint, duration, controller.TowerAiController.TargetToShoot);

        controller.TowerAiController.CooldownTrigger();
    }
}
