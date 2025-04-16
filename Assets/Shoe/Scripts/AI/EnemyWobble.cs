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

    private EnemyCharacter enemyCharacter;

    private void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;

        // Randomize the start time for variation
        timeOffset = Random.Range(0f, 100f);

        enemyCharacter = GetComponentInParent<EnemyCharacter>();
    }

    private void FixedUpdate()
    {
        Wobble();
    }

    private void Wobble()
    {
        // Calculate the wobble 
        float wobble = Mathf.Sin((Time.time + timeOffset) * wobbleSpeed * enemyCharacter.RelativeSpeedMultiplier) * wobbleAmount;

        // Calculate the tilt 
        float tilt = Mathf.Sin((Time.time + timeOffset) * tiltSpeed * enemyCharacter.RelativeSpeedMultiplier) * tiltAmount;

        // Apply the wobble to the Y position
        Vector3 newPosition = startPosition + new Vector3(0f, wobble, 0f);

        // Apply the tilt to the rotation
        Quaternion newRotation = startRotation * Quaternion.Euler(0f, 0f, tilt);

        transform.SetLocalPositionAndRotation(newPosition, newRotation);
    }
}
