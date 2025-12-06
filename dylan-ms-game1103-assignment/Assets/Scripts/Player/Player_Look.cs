using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player_Input))]
public class Player_Look : MonoBehaviour
{
    private Camera mainCam;
    private Player_Input playerInput;

    [Header("Rotation Settings")]
    [SerializeField] private float sensitivityMultiplier = 200f;

    private void Awake() {
        playerInput = GetComponent<Player_Input>();
    }
    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            enabled = false;
        }
    }

    void Update()
    {
        Vector2 mouseScreenPosition = playerInput.LookScreenPosition;

        float distanceFromCamera = transform.position.z - mainCam.transform.position.z;
        Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceFromCamera));


        Vector2 lookDir = mouseWorldPosition - transform.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        float sensitivity = 1000f;
        if (Manager_Settings.Instance != null)
        {
            sensitivity = Manager_Settings.Instance.MouseSensitivityX * sensitivityMultiplier;
        }

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, sensitivity * Time.deltaTime);
    }
}