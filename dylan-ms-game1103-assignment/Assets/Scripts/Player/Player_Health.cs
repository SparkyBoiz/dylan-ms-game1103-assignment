using UnityEngine;

/// <summary>
/// Manages the player's health, including taking damage and handling death.
/// </summary>
public class Player_Health : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("The maximum health of the player.")]
    public int maxHealth = 100;

    [Header("Visuals")]
    [Tooltip("The SpriteRenderer to change when health is low.")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Tooltip("The sprite to display when health is below the threshold.")]
    [SerializeField] private Sprite lowHealthSprite;
    [Tooltip("The health value at which the sprite will change.")]
    [SerializeField] private int lowHealthThreshold = 30;

    public int CurrentHealth => currentHealth;

    private int currentHealth;
    private Sprite defaultSprite;
    private bool isLowHealthState = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (spriteRenderer != null)
        {
            defaultSprite = spriteRenderer.sprite;
        }
        else
        {
            Debug.LogWarning("Player_Health: SpriteRenderer not assigned in the Inspector.", this);
        }
    }

    /// <summary>
    /// Reduces the player's health by a specified amount.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        CheckHealthState();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Increases the player's health by a specified amount, up to the maximum.
    /// </summary>
    /// <param name="amount">The amount of health to add.</param>
    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        CheckHealthState();
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Find the game manager and call the GameOver method.
        Manager_Game gameManager = FindObjectOfType<Manager_Game>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        // Disable the player object instead of destroying it immediately to let other scripts react.
        gameObject.SetActive(false);
    }

    private void CheckHealthState()
    {
        if (spriteRenderer == null || lowHealthSprite == null) return;

        // Check if health has dropped below the threshold
        if (currentHealth <= lowHealthThreshold && !isLowHealthState)
        {
            spriteRenderer.sprite = lowHealthSprite;
            isLowHealthState = true;
        }
        // Switch back to the default sprite if health is restored above the threshold.
        else if (currentHealth > lowHealthThreshold && isLowHealthState)
        {
            spriteRenderer.sprite = defaultSprite;
            isLowHealthState = false;
        }
    }
}