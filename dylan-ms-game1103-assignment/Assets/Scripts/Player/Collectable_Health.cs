using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectable_Health : MonoBehaviour
{
    [Header("Collectable Settings")]
    public int healthAmount = 25;

    [Header("Visual Effects")]
    public float flipSpeed = 3f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.25f;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSfx;

    private Vector3 startPos;
    private Vector3 startScale;

    void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;
    }

    void Update()
    {
        Vector3 newScale = startScale;
        newScale.x = startScale.x * Mathf.Sin(Time.time * flipSpeed);
        transform.localScale = newScale;

        Vector3 newPos = startPos;
        newPos.y += Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Health playerHealth = other.GetComponent<Player_Health>();
            if (playerHealth != null)
            {
                playerHealth.AddHealth(healthAmount);
                if (pickupSfx != null && Manager_Audio.Instance != null)
                {
                    Manager_Audio.Instance.PlaySoundAtPoint(pickupSfx, transform.position);
                }
                Destroy(gameObject);
            }
        }
    }
}