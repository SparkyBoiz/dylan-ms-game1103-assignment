using UnityEngine;
using TMPro;

public class Manager_UI : MonoBehaviour
{
    [Header("UI Text Elements")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Object References")]
    [SerializeField] private Player_Shoot playerShoot;
    [SerializeField] private Player_Health playerHealth;
    [SerializeField] private Spawner_Enemy enemySpawner;

    private float gameTimer = 0f;

    void Update()
    {
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