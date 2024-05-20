using System.Collections;
using UnityEngine;

public static class PhysicsMethods
{
    public static IEnumerator Throwing(Transform objectToThrow, Vector3 targetTransform, float dragSpeed, float distanceLimit, bool lookAt)
    {
        bool throwing = true;
        float distanceToTarget = Vector3.Distance(objectToThrow.transform.position, targetTransform);
        while (throwing)
        {
            Debug.Log("throwing");
            if (lookAt)
                objectToThrow.transform.LookAt(targetTransform);

            // Calculate the angle in the arc
            float angle = Mathf.Min(1, Vector3.Distance(objectToThrow.transform.position, targetTransform) / distanceToTarget) * 45;
            objectToThrow.transform.rotation = objectToThrow.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);

            float currentDist = Vector3.Distance(objectToThrow.transform.position, targetTransform);
            if (currentDist < distanceLimit)
                throwing = false;           
            else
                objectToThrow.transform.Translate(Vector3.forward * Mathf.Min(dragSpeed * Time.deltaTime, currentDist));

            yield return null;
        }
        Debug.Log("done throwing");
    }

    public static IEnumerator SimulateProjectile(Transform objectToThrow, Transform targetPosition, Transform originalPosition, float angle, float duration)
    {
        // Move projectile to the position of throwing object + add some offset if needed.
        objectToThrow.transform.position = originalPosition.transform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(objectToThrow.transform.position, targetPosition.transform.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / 9.8f);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration;
        if (duration == 0)
            flightDuration = target_Distance / Vx;
        else flightDuration = duration;

        // Rotate projectile to face the target.
        objectToThrow.transform.rotation = Quaternion.LookRotation(targetPosition.transform.position - objectToThrow.transform.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration && objectToThrow.transform != null)
        {
            objectToThrow.transform.Translate(0, (Vy - (9.8f * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
