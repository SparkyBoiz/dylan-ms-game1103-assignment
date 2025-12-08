using UnityEngine;

public class Player_Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite lowHealthSprite;
    [SerializeField] private int lowHealthThreshold = 30;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSfx;

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

        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        CheckHealthState();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        CheckHealthState();
    }

    private void Die()
    {
        if (deathSfx != null && Manager_Audio.Instance != null)
        {
            // Play at camera position for consistent volume regardless of player location
            Manager_Audio.Instance.PlaySoundAtPoint(deathSfx, Camera.main.transform.position);
        }

        Manager_Game gameManager = FindObjectOfType<Manager_Game>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        gameObject.SetActive(false);
    }

    private void CheckHealthState()
    {
        if (spriteRenderer == null || lowHealthSprite == null) return;

        if (currentHealth <= lowHealthThreshold && !isLowHealthState)
        {
            spriteRenderer.sprite = lowHealthSprite;
            isLowHealthState = true;
        }
        else if (currentHealth > lowHealthThreshold && isLowHealthState)
        {
            spriteRenderer.sprite = defaultSprite;
            isLowHealthState = false;
        }
    }
}