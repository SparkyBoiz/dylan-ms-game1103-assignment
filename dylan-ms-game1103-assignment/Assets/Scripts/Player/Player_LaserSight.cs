using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Player_LaserSight : MonoBehaviour
{
    public Transform firePoint;

    public LayerMask collisionLayers;

    public float maxDistance = 100f;

    public Color laserColor = Color.red;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

        void Update()
    {
        UpdateLaserColor();

        lineRenderer.SetPosition(0, firePoint.position);

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.up, maxDistance, collisionLayers);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, firePoint.position + firePoint.up * maxDistance);
        }
    }

    private void UpdateLaserColor()
    {
        if (lineRenderer == null) return;
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
    }
}