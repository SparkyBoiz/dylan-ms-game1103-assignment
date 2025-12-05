using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This component handles the player's camera rotation based on mouse input.
/// </summary>
public class PlayerLookController : MonoBehaviour
{
    [Header("Object References")]
    [Tooltip("The transform of the player's body, used for horizontal rotation.")]
    [SerializeField] private Transform _playerBody;

    private float _sensitivityX = 1.0f;
    private float _sensitivityY = 1.0f;
    private float _xRotation = 0f;

    private void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks when this object is destroyed

    }

    private void Update()
    {
        // Get mouse input, scaled by sensitivity and time
        float mouseX = Input.GetAxis("Mouse X") * _sensitivityX * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivityY * 100f * Time.deltaTime;

        // Calculate vertical rotation and clamp it to prevent flipping
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        // Apply rotation
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f); // Vertical rotation on the camera
        _playerBody.Rotate(Vector3.up * mouseX); // Horizontal rotation on the player body
    }
}