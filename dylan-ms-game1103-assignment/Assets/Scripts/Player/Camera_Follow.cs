using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target;

    [Range(0f, 1f)]
    public float smoothTime = 0.3f;

    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}