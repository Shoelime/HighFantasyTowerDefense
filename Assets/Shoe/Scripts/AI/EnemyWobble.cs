using UnityEngine;

public class EnemyWobble : MonoBehaviour
{
    [SerializeField] private float wobbleSpeed = 20f; 
    [SerializeField] private float wobbleAmount = 10f; 
    [SerializeField] private float tiltAmount = 5f; 
    [SerializeField] private float tiltSpeed = 20f; 

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float timeOffset;

    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        timeOffset = Random.Range(0f, 100f); // Randomize the start time for variation
    }

    void FixedUpdate()
    {
        Wobble();
    }

    void Wobble()
    {
        // Calculate the wobble based on sine wave
        float wobble = Mathf.Sin((Time.time + timeOffset) * wobbleSpeed) * wobbleAmount;

        // Calculate the tilt based on sine wave
        float tilt = Mathf.Sin((Time.time + timeOffset) * tiltSpeed) * tiltAmount;

        // Apply the wobble to the Y position
        Vector3 newPosition = startPosition + new Vector3(0f, wobble, 0f);

        // Apply the tilt to the rotation
        Quaternion newRotation = startRotation * Quaternion.Euler(0f, 0f, tilt);

        // Apply the new position and rotation
        transform.localPosition = newPosition;
        transform.localRotation = newRotation;
    }
}
