using UnityEngine;

/// <summary>
/// Makes the camera smoothly follow a target transform.
/// Ideal for a top-down 2D game.
/// </summary>
public class Camera_Follow : MonoBehaviour
{
    [Tooltip("The target the camera should follow (e.g., the player).")]
    public Transform target;

    [Tooltip("Approximately the time it will take to reach the target. A smaller value will reach the target faster.")]
    [Range(0f, 1f)]
    public float smoothTime = 0.3f;

    [Tooltip("The offset from the target (especially the Z-axis for top-down view).")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // The desired position for the camera is the target's position plus the offset.
        Vector3 targetPosition = target.position + offset;
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}