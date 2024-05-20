using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    public Axis rotationAxis = Axis.Y; // Default rotation axis is Y
    public float rotationSpeed = 30f; // Default rotation speed

    private void Update()
    {
        // Calculate the rotation vector based on the chosen axis and speed
        Vector3 rotationVector = Vector3.zero;
        switch (rotationAxis)
        {
            case Axis.X:
                rotationVector = Vector3.right;
                break;
            case Axis.Y:
                rotationVector = Vector3.up;
                break;
            case Axis.Z:
                rotationVector = Vector3.forward;
                break;
        }

        // Rotate the object continuously around the chosen axis
        transform.Rotate(rotationVector, rotationSpeed * Time.deltaTime);
    }
}
