using UnityEngine;

public static class ProjectileCalculator
{
    public static Vector3 CalculateInterceptPoint(Vector3 startingPosition, Vector3 targetPosition, Vector3 targetsPreviousPosition, float deltaTime, float projectileSpeed)
    {
        // Calculate enemy velocity based on previous position
        Vector3 enemyVelocity = (targetPosition - targetsPreviousPosition) / deltaTime;

        // Calculate time to intercept
        float distanceToTarget = Vector3.Distance(startingPosition, targetPosition);
        float timeToIntercept = distanceToTarget / projectileSpeed;

        // Calculate intercept point
        Vector3 interceptPoint = targetPosition + (enemyVelocity * timeToIntercept / 2f);

        return interceptPoint;
    }

    public static float CalculateShotDuration(Vector3 startPosition, Vector3 targetPosition, float projectileSpeed)
    {
        float distanceToTarget = Vector3.Distance(startPosition, targetPosition);
        float shotDuration = distanceToTarget / projectileSpeed;
        return shotDuration;
    }
}
