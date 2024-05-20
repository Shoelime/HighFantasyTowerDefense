using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/AI/ShootTarget")]

public class TowerShootTargetAction : StateAction
{
    public override void Act(StateController controller)
    {
        if (controller.TowerAiController.TargetToShoot == null || controller.TowerAiController.AttackCooldownActive)
        {
            return;
        }

        ShootProjectile(controller);
        controller.TowerAiController.CooldownTrigger();
    }

    void ShootProjectile(StateController controller)
    {
        Vector3 previousPosition = controller.TowerAiController.TargetToShoot.PreviousPosition;
        Vector3 currentPosition = controller.TowerAiController.TargetToShoot.transform.position;
       
        var projectile = controller.TowerAiController.TowerData.ProjectileToShoot.Get<Projectile>(controller.TowerAiController.transform.position + controller.TowerAiController.ProjectileStartPosition, Quaternion.identity);

        Vector3 interceptPoint = MathUtils.CalculateInterceptPoint(
            projectile.transform.position, 
            currentPosition, 
            previousPosition, 
            controller.TowerAiController.TowerData.ProjectileSpeed, 
            out float duration);

        projectile.CallCoroutine(projectile.transform.position, interceptPoint, duration, controller.TowerAiController.TargetToShoot);
    }
}
