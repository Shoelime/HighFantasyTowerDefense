using UnityEngine;

public static class MathUtils
{
    public static float MapValue(float mainValue, float inValueMin, float inValueMax, float outValueMin, float outValueMax)
    {
        return (mainValue - inValueMin) * (outValueMax - outValueMin) / (inValueMax - inValueMin) + outValueMin;
    }

    public static float RemapClamped(float val, float in1, float in2, float out1, float out2,
         bool in1Clamped, bool in2Clamped, bool out1Clamped, bool out2Clamped)
    {
        if (in1Clamped == true && val < in1) val = in1;
        if (in2Clamped == true && val > in2) val = in2;

        float result = out1 + (val - in1) * (out2 - out1) / (in2 - in1);

        if (out1Clamped == true && result < out1) result = out1;
        if (out2Clamped == true && result > out2) result = out2;

        return result;
    }

    public static float AngleBetweenQuaternions(Quaternion rotationA, Quaternion rotationB)
    {
        // get a "forward vector" for each rotation
        var forwardA = rotationA * Vector3.forward;
        var forwardB = rotationB * Vector3.forward;

        // get a numeric angle for each vector, on the X-Z plane (relative to world forward)
        var angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
        var angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;

        // get the signed difference in these angles
        float angleDiff = Mathf.DeltaAngle(angleA, angleB);
        return angleDiff;
    }

    public static Vector3 EulerAnglesBetween(Quaternion from, Quaternion to)
    {
        Vector3 delta = to.eulerAngles - from.eulerAngles;

        if (delta.x > 180)
            delta.x -= 360;
        else if (delta.x < -180)
            delta.x += 360;

        if (delta.y > 180)
            delta.y -= 360;
        else if (delta.y < -180)
            delta.y += 360;

        if (delta.z > 180)
            delta.z -= 360;
        else if (delta.z < -180)
            delta.z += 360;

        return delta;
    }

    public static void SphericalAlign(Vector3 center, ref Transform target)
    {
        Vector3 _relativePos = center - target.position;
        Quaternion _rotation = Quaternion.LookRotation(_relativePos, -Vector3.up);

        target.position = _relativePos;
        target.rotation = _rotation;
        target.Rotate(new Vector3(1, 0, 0), -90f, Space.Self);
    }

    public static bool IsApproximately(float a, float b, float threshold)
    {
        if (threshold > 0f)
        {
            return Mathf.Abs(a - b) <= threshold;
        }
        else
        {
            return Mathf.Approximately(a, b);
        }
    }

    public static bool Vector3IsApproximately(Vector3 a, Vector3 b, float threshold)
    {
        return IsApproximately(a.magnitude, b.magnitude, threshold);
    }

    public static bool QuaternionIsApproximately(Quaternion a, Quaternion b, float threshold)
    {
        float angle = AngleBetweenQuaternions(a, b);
        return IsApproximately(angle, 0f, threshold);
    }

    public static bool IsFacingObject(Transform forwardT, Transform positionT)
    {
        Vector3 forward = forwardT.forward;
        Vector3 toOther = (positionT.position - forwardT.position).normalized;

        if (Vector3.Dot(forward, toOther) < 0f)   // not facing
            return false;

        return true;
    }

    // Calculate intercept point for shooting a moving enemy
    public static Vector3 CalculateInterceptPoint(Vector3 startingPosition, Vector3 targetPosition, Vector3 targetPreviousPosition, float projectileSpeed, out float duration)
    {
        // Calculate velocity of the target
        Vector3 targetVelocity = (targetPosition - targetPreviousPosition) / Time.deltaTime;

        // Calculate time to intercept
        float distanceToTarget = Vector3.Distance(startingPosition, targetPosition);
        float timeToIntercept = distanceToTarget / projectileSpeed;
        duration = timeToIntercept;

        // Calculate intercept point
        Vector3 interceptPoint = targetPosition + (targetVelocity * timeToIntercept);

        return interceptPoint;
    }

    // Calculate shot duration based on distance and projectile speed
    public static float CalculateProjectileFlightDuration(Vector3 startPosition, Vector3 targetPosition, float projectileSpeed)
    {
        float distanceToTarget = Vector3.Distance(startPosition, targetPosition);
        float shotDuration = distanceToTarget / projectileSpeed;
        return shotDuration;
    }

    public static bool HasLineOfSight(Vector3 startingTransform, Transform targetTransform, LayerMask obstacleLayers, float distance)
    {
        Vector3 directionToTarget = targetTransform.position + Vector3.up - startingTransform;
        //Debug.DrawRay(startingTransform, directionToTarget, Color.red,  distance);

        if (Physics.Raycast(startingTransform, directionToTarget, out RaycastHit hit, distance))
        {
            if (hit.collider.transform == targetTransform)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static GameObject FindClosestGameObject(Vector3 position, GameObject[] gameObjects)
    {
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogWarning("No gameObjects provided to find the closest one.");
            return null;
        }

        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in gameObjects)
        {
            if (obj != null && obj.activeSelf)
            {
                float distance = Vector3.Distance(position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestObject = obj;
                    closestDistance = distance;
                }
            }
        }

        return closestObject;
    }
}
