using UnityEngine;

public class MoveObjectToPosition : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float duration = 1.0f;

    private Vector3 startPosition;
    private float startTime;
    private bool moving;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        if (moving)
            MovingObject();
    }

    private void MovingObject()
    {
        // Calculate the elapsed time since the movement started
        float elapsedTime = Time.time - startTime * Time.timeScale;

        // Ensure the movement doesn't exceed the specified duration
        float t = Mathf.Clamp01(elapsedTime / duration);

        // Apply smooth step to the interpolation factor for smoother acceleration and deceleration
        t = Mathf.SmoothStep(0f, 1f, t);

        // Lerp from the starting position to the target position based on the interpolation factor
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1.0f)
        {
            moving = false;
        }
    }

    public void StartMovement()
    {
        startTime = Time.time;

        moving = true;
    }
}
