using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float xMinLimit;
    [SerializeField] private float yMinLimit;
    [SerializeField] private float zMinLimit;
    [SerializeField] private float xMaxLimit;
    [SerializeField] private float yMaxLimit;
    [SerializeField] private float zMaxLimit;

    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float zoomSpeed = 10f;

    private IGameStateHandler gameState;
    public float debugValue;

    private void Start()
    {
        gameState = Services.Get<IGameStateHandler>();
    }

    void Update()
    {
        if (gameState.GetCurrentGameState == GameState.Paused)
            return;

        HandleCameraMovement();
        HandleCameraZoom();
    }

    private void HandleCameraMovement()
    {
        // Get input from WASD or arrow keys
        float moveX = Input.GetAxisRaw("Horizontal");  // A/D or Left/Right arrow keys
        float moveZ = Input.GetAxisRaw("Vertical");    // W/S or Up/Down arrow keys

        // Calculate the input direction
        Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Transform the input direction to the camera's orientation
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;  // Ignore vertical component
        cameraRight.y = 0;    // Ignore vertical component
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = inputDirection.z * cameraForward + inputDirection.x * cameraRight;

        // Calculate the new position
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // Clamp the new position within the defined limits
        newPosition.x = Mathf.Clamp(newPosition.x, xMinLimit, xMaxLimit);
        newPosition.y = Mathf.Clamp(newPosition.y, yMinLimit, yMaxLimit);
        newPosition.z = Mathf.Clamp(newPosition.z, zMinLimit, zMaxLimit);

        // Move the camera to the new position
        transform.position = newPosition;
    }

    private void HandleCameraZoom()
    {
        // Get input from mouse scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        debugValue = scrollInput;
        // Calculate the new zoom position
        float newZoom = transform.position.y - scrollInput * zoomSpeed * Time.deltaTime;

        // Clamp the new zoom position within the defined Y limits
        newZoom = Mathf.Clamp(newZoom, yMinLimit, yMaxLimit);

        // Apply the new zoom position
        transform.position = new Vector3(transform.position.x, newZoom, transform.position.z);
    }
}
