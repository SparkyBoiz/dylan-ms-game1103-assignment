using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns ammo collectables at a regular interval at its transform position.
/// </summary>
public class Spawner_Ammo : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("The ammo collectable prefab to spawn.")]
    public GameObject ammoPrefab;

    [Tooltip("The list of possible points where the ammo can spawn. A random one will be chosen. If empty, the spawner's own position will be used.")]
    public Transform[] spawnPoints;

    [Tooltip("The time in seconds between spawns after the previous one is collected.")]
    public float spawnInterval = 10f;

    private GameObject spawnedAmmo;

    void Start()
    {
        // Immediately spawn the first ammo pickup.
        SpawnAmmo();
    }

    void Update()
    {
        // If the spawned ammo has been collected (destroyed), start a timer to spawn a new one.
        if (spawnedAmmo == null)
        {
            Invoke(nameof(SpawnAmmo), spawnInterval);
            // Disable this script to prevent Invoke from being called repeatedly.
            // It will be re-enabled when the new ammo spawns.
            enabled = false;
        }
    }

    private void SpawnAmmo()
    {
        Vector3 spawnPosition;

        // Check if spawn points are assigned and use a random one.
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            spawnPosition = spawnPoints[spawnIndex].position;
        }
        else
        {
            // Fallback to the spawner's own position if no spawn points are set.
            Debug.LogWarning("Spawner_Ammo: No spawn points assigned. Using spawner's position as fallback.");
            spawnPosition = transform.position;
        }

        spawnedAmmo = Instantiate(ammoPrefab, spawnPosition, Quaternion.identity);
        // Re-enable the script to check when the new pickup is collected.
        enabled = true;
    }
}