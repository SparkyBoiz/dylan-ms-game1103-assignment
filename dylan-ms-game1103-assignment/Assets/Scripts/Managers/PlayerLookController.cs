using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This component handles the player's camera rotation based on mouse input.
/// It retrieves sensitivity settings from the Manager_Settings singleton.
/// </summary>
public class PlayerLookController : MonoBehaviour
{
    [Header("Object References")]
    [Tooltip("The transform of the player's body, used for horizontal rotation.")]
    [SerializeField] private Transform _playerBody;

    // These are kept to receive updates from Manager_Settings, but are not used in the rotation logic.
    // They could be repurposed for other mechanics.
    private float _sensitivityX;
    private float _sensitivityY;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        // For a top-down game, the cursor should be visible and confined.
        Cursor.lockState = CursorLockMode.Confined;

        // Apply initial sensitivity settings
        UpdateSensitivities();

        // Subscribe to settings changes to update sensitivity in real-time
        if (Manager_Settings.Instance != null)
        {
            Manager_Settings.Instance.OnSettingsChanged += OnSettingsChanged;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks when this object is destroyed
        if (Manager_Settings.Instance != null)
        {
            Manager_Settings.Instance.OnSettingsChanged -= OnSettingsChanged;
        }
    }

    private void Update()
    {
        // Create a plane at the player's position with a normal pointing up.
        Plane playerPlane = new Plane(Vector3.up, _playerBody.position);

        // Create a ray from the camera to the mouse cursor.
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the ray intersects the plane.
        if (playerPlane.Raycast(ray, out float hitDist))
        {
            // Get the point of intersection.
            Vector3 targetPoint = ray.GetPoint(hitDist);

            // Create a rotation that looks at the target point from the player's position.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - _playerBody.position);

            // Keep the player upright by removing rotation on the X and Z axes.
            targetRotation.x = 0;
            targetRotation.z = 0;

            _playerBody.rotation = targetRotation;
        }
    }

    private void OnSettingsChanged(SettingType type)
    {
        if (type == SettingType.MouseX || type == SettingType.MouseY)
        {
            UpdateSensitivities();
        }
    }

    private void UpdateSensitivities()
    {
        if (Manager_Settings.Instance == null) return;
        _sensitivityX = Manager_Settings.Instance.MouseSensitivityX;
        _sensitivityY = Manager_Settings.Instance.MouseSensitivityY;
        Debug.Log($"Mouse sensitivity updated: X={_sensitivityX}, Y={_sensitivityY}");
    }

}

