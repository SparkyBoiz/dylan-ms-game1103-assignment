using UnityEngine;

/// <summary>
/// Creates a laser sight effect from the fire point to where the player is aiming.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class Player_LaserSight : MonoBehaviour
{
    [Tooltip("The point from which the laser is fired. Should be the same as the fire point in Player_Shoot.")]
    public Transform firePoint;

    [Tooltip("The layers that the laser will collide with.")]
    public LayerMask collisionLayers;

    [Tooltip("The maximum distance the laser will travel.")]
    public float maxDistance = 100f;

    [Tooltip("The color of the laser sight.")]
    public Color laserColor = Color.red;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

        void Update()
    {
        // Ensure the color is set correctly every frame.
        UpdateLaserColor();

        // Set the starting position of the laser to the fire point.
        lineRenderer.SetPosition(0, firePoint.position);

        // Cast a ray from the fire point in the direction the player is facing.
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.up, maxDistance, collisionLayers);

        if (hit.collider != null)
        {
            // If the ray hits something, set the end of the laser to the hit point.
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // If the ray doesn't hit anything, set the end of the laser to its maximum distance.
            lineRenderer.SetPosition(1, firePoint.position + firePoint.up * maxDistance);
        }
    }

    /// <summary>
    /// Applies the laserColor to the LineRenderer.
    /// </summary>
    private void UpdateLaserColor()
    {
        if (lineRenderer == null) return;
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
    }
}