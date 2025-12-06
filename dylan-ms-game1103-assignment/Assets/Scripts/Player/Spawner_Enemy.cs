using UnityEngine;
using System.Collections;
public class Spawner_Enemy : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject[] enemyPrefabs;
        public int count;
        public float spawnRate;
    }

    [Header("Spawner Settings")]
    public Wave[] waves;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;

    public int CurrentWaveNumber => currentWaveIndex + 1;
    public int TotalWaves => waves.Length;
    public int EnemiesAlive => enemiesAlive;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;

    void OnEnable()
    {
        Enemy_Health.OnEnemyKilled += HandleEnemyKilled;
    }

    void OnDisable()
    {
        Enemy_Health.OnEnemyKilled -= HandleEnemyKilled;
    }

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            enabled = false;
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(2f);

        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            yield return StartCoroutine(SpawnWave(currentWave));

            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveIndex++;
        }

        Manager_Game gameManager = FindObjectOfType<Manager_Game>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {

        if (wave.enemyPrefabs == null || wave.enemyPrefabs.Length == 0)
        {
            yield break;
        }

        enemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            GameObject enemyToSpawn = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
            
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        while (enemiesAlive > 0)
        {
            yield return null;
        }
    }

    private void HandleEnemyKilled()
    {
        enemiesAlive--;
    }
}