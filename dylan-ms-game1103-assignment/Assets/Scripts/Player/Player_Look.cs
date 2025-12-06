using UnityEngine;
using UnityEngine.InputSystem;

/// <summary> /// Rotates the player to face the mouse cursor in a 2D top-down view. /// </summary>
[RequireComponent(typeof(Player_Input))]
public class Player_Look : MonoBehaviour
{
    private Camera mainCam;
    private Player_Input playerInput;

    [Header("Rotation Settings")]
    [Tooltip("A multiplier to make the slider value more responsive. Adjust this in the inspector.")]
    [SerializeField] private float sensitivityMultiplier = 200f;

    private void Awake() {
        playerInput = GetComponent<Player_Input>();
    }
    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("PlayerLook Script: No main camera found in the scene. Please tag your camera as 'MainCamera'.");
            enabled = false; // Disable the script if no camera is found.
        }
    }

    void Update()
    {
        // Get mouse position from our central input script.
        Vector2 mouseScreenPosition = playerInput.LookScreenPosition;

        // To get the correct world position in a 2D game, we need to provide a Z-coordinate.
        // We calculate the distance from the camera to the game plane (at Z=0).
        float distanceFromCamera = transform.position.z - mainCam.transform.position.z;
        Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceFromCamera));


        // Calculate the direction from the player to the mouse.
        Vector2 lookDir = mouseWorldPosition - transform.position;

        // Calculate the angle and convert it to degrees. The -90f offset is to align the sprite's 'up' direction.
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        // Get sensitivity from settings, with a fallback default.
        float sensitivity = 1000f; // A sensible default if the settings manager isn't found.
        if (Manager_Settings.Instance != null)
        {
            // We multiply the slider value to make it feel more responsive.
            // You can now adjust this multiplier in the Inspector.
            sensitivity = Manager_Settings.Instance.MouseSensitivityX * sensitivityMultiplier;
        }

        // Create the target rotation.
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        // Rotate towards the target at a max speed of `sensitivity` degrees per second.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, sensitivity * Time.deltaTime);
    }
}