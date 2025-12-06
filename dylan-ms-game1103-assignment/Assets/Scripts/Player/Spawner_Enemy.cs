using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns waves of enemies at a set of spawn points.
/// </summary>
public class Spawner_Enemy : MonoBehaviour
{
    /// <summary>
    /// Defines a single wave of enemies, including the prefab, count, and spawn rate.
    /// </summary>
    [System.Serializable]
    public class Wave
    {
        public string name;
        [Tooltip("The different enemy prefabs that can spawn in this wave. A random one will be chosen for each spawn.")]
        public GameObject[] enemyPrefabs;
        public int count;
        [Tooltip("The number of enemies to spawn per second.")]
        public float spawnRate;
    }

    [Header("Spawner Settings")]
    [Tooltip("An array defining the sequence of enemy waves.")]
    public Wave[] waves;

    [Tooltip("An array of transforms representing the possible spawn locations.")]
    public Transform[] spawnPoints;

    [Tooltip("The time in seconds to wait between waves.")]
    public float timeBetweenWaves = 5f;

    public int CurrentWaveNumber => currentWaveIndex + 1;
    public int TotalWaves => waves.Length;
    public int EnemiesAlive => enemiesAlive;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;

    void OnEnable()
    {
        // Subscribe to the enemy killed event.
        Enemy_Health.OnEnemyKilled += HandleEnemyKilled;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks.
        Enemy_Health.OnEnemyKilled -= HandleEnemyKilled;
    }

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Spawner_Enemy: No spawn points referenced. Please assign spawn points in the Inspector.");
            enabled = false;
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    /// <summary>
    /// The main coroutine that manages the lifecycle of spawning waves.
    /// </summary>
    private IEnumerator SpawnWaves()
    {
        // Wait for a moment at the start before the first wave begins.
        yield return new WaitForSeconds(2f);

        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            yield return StartCoroutine(SpawnWave(currentWave));

            // Wait for a delay before the next wave.
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveIndex++;
        }

        Debug.Log("All waves completed!");
        // Find the game manager and call the GameOver method.
        Manager_Game gameManager = FindObjectOfType<Manager_Game>();
        if (gameManager != null)
        {
            // You might want a "YouWin" scene here, but for now we'll use GameOver.
            gameManager.GameOver();
        }
    }

    /// <summary>
    /// Spawns a single wave of enemies and waits until they are all defeated.
    /// </summary>
    private IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.name);

        if (wave.enemyPrefabs == null || wave.enemyPrefabs.Length == 0)
        {
            Debug.LogWarning($"Wave '{wave.name}' has no enemy prefabs assigned. Skipping wave.");
            yield break; // Exit the coroutine for this wave if no prefabs are assigned.
        }

        enemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            GameObject enemyToSpawn = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
            
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        // Wait until all enemies in the current wave are defeated.
        while (enemiesAlive > 0)
        {
            yield return null; // Wait for the next frame.
        }
    }

    /// <summary>
    /// Handles the event when an enemy is killed, decrementing the alive count.
    /// </summary>
    private void HandleEnemyKilled()
    {
        enemiesAlive--;
    }
}