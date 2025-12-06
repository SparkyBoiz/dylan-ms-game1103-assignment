using UnityEngine;
using TMPro; // Make sure to import the TextMeshPro namespace

/// <summary>
/// Manages and updates all primary UI elements for the player HUD.
/// </summary>
public class Manager_UI : MonoBehaviour
{
    [Header("UI Text Elements")]
    [Tooltip("Text element to display player's ammo.")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [Tooltip("Text element to display player's health.")]
    [SerializeField] private TextMeshProUGUI healthText;
    [Tooltip("Text element to display the current wave number.")]
    [SerializeField] private TextMeshProUGUI waveText;
    [Tooltip("Text element to display the number of enemies remaining.")]
    [SerializeField] private TextMeshProUGUI enemiesText;
    [Tooltip("Text element to display the game timer.")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Object References")]
    [Tooltip("Reference to the Player_Shoot script.")]
    [SerializeField] private Player_Shoot playerShoot;
    [Tooltip("Reference to the Player_Health script.")]
    [SerializeField] private Player_Health playerHealth;
    [Tooltip("Reference to the Spawner_Enemy script.")]
    [SerializeField] private Spawner_Enemy enemySpawner;

    private float gameTimer = 0f;

    void Update()
    {
        // Continuously update all UI elements each frame.
        UpdateAmmoUI();
        UpdateHealthUI();
        UpdateWaveUI();
        UpdateTimerUI();
    }

    private void UpdateAmmoUI()
    {
        if (playerShoot != null && ammoText != null)
        {
            if (playerShoot.IsReloading)
            {
                ammoText.text = "Reloading...";
            }
            else
            {
                ammoText.text = $"Ammo: {playerShoot.CurrentAmmo} / {playerShoot.maxAmmo}";
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (playerHealth != null && healthText != null)
        {
            // Ensure health doesn't display as negative.
            int displayHealth = Mathf.Max(0, playerHealth.CurrentHealth);
            healthText.text = $"Health: {displayHealth}";
        }
    }

    private void UpdateWaveUI()
    {
        if (enemySpawner != null)
        {
            if (waveText != null)
                waveText.text = $"Wave: {enemySpawner.CurrentWaveNumber} / {enemySpawner.TotalWaves}";

            if (enemiesText != null)
                enemiesText.text = $"Enemies: {enemySpawner.EnemiesAlive}";
        }
    }

    private void UpdateTimerUI()
    {
        gameTimer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gameTimer / 60F);
        int seconds = Mathf.FloorToInt(gameTimer % 60F);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}