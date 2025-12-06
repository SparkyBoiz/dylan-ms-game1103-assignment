using UnityEngine;

/// <summary>
/// A collectable item that gives the player a specified amount of health and has a bobbing/flipping animation.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Collectable_Health : MonoBehaviour
{
    [Header("Collectable Settings")]
    [Tooltip("The amount of health to give the player.")]
    public int healthAmount = 25;

    [Header("Visual Effects")]
    [Tooltip("How fast the pickup flips back and forth.")]
    public float flipSpeed = 3f;
    [Tooltip("How fast the pickup bobs up and down.")]
    public float bobSpeed = 2f;
    [Tooltip("How high the pickup bobs from its starting position.")]
    public float bobHeight = 0.25f;

    private Vector3 startPos;
    private Vector3 startScale;

    void Start()
    {
        // Store the starting position for the bobbing calculation.
        startPos = transform.position;
        // Store the starting scale for the flipping calculation.
        startScale = transform.localScale;
    }

    void Update()
    {
        // Flip the object back and forth on its X-axis using a sine wave.
        Vector3 newScale = startScale;
        newScale.x = startScale.x * Mathf.Sin(Time.time * flipSpeed);
        transform.localScale = newScale;

        // Bob the object up and down using a sine wave for a smooth floating effect.
        Vector3 newPos = startPos;
        newPos.y += Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player.
        if (other.CompareTag("Player"))
        {
            Player_Health playerHealth = other.GetComponent<Player_Health>();
            if (playerHealth != null)
            {
                playerHealth.AddHealth(healthAmount);
                Destroy(gameObject);
            }
        }
    }
}