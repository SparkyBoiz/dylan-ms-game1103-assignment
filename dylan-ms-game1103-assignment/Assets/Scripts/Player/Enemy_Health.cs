using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/// <summary>
/// Manages the enemy's health, including taking damage and handling death.
/// </summary>
public class Enemy_Health : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        [Tooltip("The pickup prefab to spawn.")]
        public GameObject itemPrefab;

        [Tooltip("The chance (from 0 to 100) that this item will drop.")]
        [Range(0f, 100f)]
        public float dropChance;
    }

    [Header("Health Settings")]
    [Tooltip("The maximum health of the enemy.")]
    public int maxHealth = 50;

    [Header("Visuals")]
    [Tooltip("The SpriteRenderer to change when health is low.")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Tooltip("The sprite to display when health is below the threshold.")]
    [SerializeField] private Sprite lowHealthSprite;
    [Tooltip("The health value at which the sprite will change.")]
    [SerializeField] private int lowHealthThreshold = 15;

    [Header("Loot")]
    [Tooltip("A list of possible items that can be dropped by this enemy.")]
    [SerializeField] private List<LootItem> lootTable = new List<LootItem>();

    // A static event that other scripts can subscribe to.
    public static event Action OnEnemyKilled;

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
            Debug.LogWarning("Enemy_Health: SpriteRenderer not assigned in the Inspector.", this);
        }
    }

    /// <summary>
    /// Reduces the enemy's health by a specified amount.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        CheckHealthState();

        if (currentHealth <= 0)
        {
            // Invoke the event before destroying the object.
            OnEnemyKilled?.Invoke();
            HandleLootDrop();
            Destroy(gameObject);
        }
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
        // Optional: Add logic here to switch back if the enemy is healed.
    }

    private void HandleLootDrop()
    {
        if (lootTable == null || lootTable.Count == 0) return;

        foreach (var lootItem in lootTable)
        {
            if (lootItem.itemPrefab == null) continue;

            // For each item in the loot table, roll a chance to see if it drops.
            float randomChance = Random.Range(0f, 100f);
            if (randomChance <= lootItem.dropChance)
            {
                Instantiate(lootItem.itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}