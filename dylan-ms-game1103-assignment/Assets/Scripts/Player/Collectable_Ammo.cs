using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectable_Ammo : MonoBehaviour
{
    [Header("Collectable Settings")]
    public int ammoAmount = 15;

    [Header("Visual Effects")]
    public float flipSpeed = 3f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.25f;

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
            Player_Shoot playerShoot = other.GetComponent<Player_Shoot>();
            if (playerShoot != null)
            {
                playerShoot.AddAmmo(ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}