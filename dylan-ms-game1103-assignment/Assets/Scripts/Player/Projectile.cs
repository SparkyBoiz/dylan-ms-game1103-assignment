using UnityEngine;

/// <summary>
/// Defines the behavior of a projectile. It moves forward and damages enemies on collision.
/// </summary> 
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [Tooltip("The speed at which the projectile travels.")]
    public float speed = 20f;
    [Tooltip("The amount of damage the projectile deals.")]
    public int damage = 10;
    [Tooltip("The lifetime of the projectile in seconds before it is automatically destroyed.")]
    public float lifetime = 3f;

    [Tooltip("Set to true if this projectile is fired by an enemy.")]
    public bool isEnemyProjectile = false;

    void Start()
    {
        // Destroy the projectile after its lifetime expires to prevent clutter.
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile forward ("up" relative to its rotation) each frame.
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    // This function is called when the object enters a trigger collider.
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // If the projectile hits an object on the "Environment" layer, destroy it.
        // This must be checked before checking for enemies or the player.
        if (hitInfo.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            Destroy(gameObject);
            return; // Exit the function to prevent further collision checks
        }

        if (isEnemyProjectile)
        {
            // If it's an enemy projectile, it should only damage the player.
            Player_Health player = hitInfo.GetComponent<Player_Health>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            // If it's a player projectile, it should only damage enemies.
            Enemy_Health enemy = hitInfo.GetComponent<Enemy_Health>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}