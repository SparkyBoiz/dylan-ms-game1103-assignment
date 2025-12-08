using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Enemy_Health : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject itemPrefab;

        [Range(0f, 100f)]
        public float dropChance;
    }

    [Header("Health Settings")]
    public int maxHealth = 50;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite lowHealthSprite;
    [SerializeField] private int lowHealthThreshold = 15;

    [Header("Loot")]
    [SerializeField] private List<LootItem> lootTable = new List<LootItem>();

    [Header("Audio")]
    [SerializeField] private AudioClip deathSfx;

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
            
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        CheckHealthState();

        if (currentHealth <= 0)
        {
            if (deathSfx != null && Manager_Audio.Instance != null) 
            {
                Manager_Audio.Instance.PlaySoundAtPoint(deathSfx, transform.position);
                Debug.Log("Enemy died!");
            }

            OnEnemyKilled?.Invoke();
            Debug.Log("Enemy killed!");
            HandleLootDrop();
            Destroy(gameObject);
        }
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

    private void HandleLootDrop()
    {
        if (lootTable == null || lootTable.Count == 0) return;

        foreach (var lootItem in lootTable)
        {
            if (lootItem.itemPrefab == null) continue;

            float randomChance = Random.Range(0f, 100f);
            if (randomChance <= lootItem.dropChance)
            {
                Instantiate(lootItem.itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}