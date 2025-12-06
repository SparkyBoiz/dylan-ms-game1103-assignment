using UnityEngine;
using System.Collections;

public class Spawner_Ammo : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject ammoPrefab;

    public Transform[] spawnPoints;

    public float spawnInterval = 10f;

    private GameObject spawnedAmmo;

    void Start()
    {
        SpawnAmmo();
    }

    void Update()
    {
        if (spawnedAmmo == null)
        {
            Invoke(nameof(SpawnAmmo), spawnInterval);
            enabled = false;
        }
    }

    private void SpawnAmmo()
    {
        Vector3 spawnPosition;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            spawnPosition = spawnPoints[spawnIndex].position;
        }
        else
        {
            spawnPosition = transform.position;
        }

        spawnedAmmo = Instantiate(ammoPrefab, spawnPosition, Quaternion.identity);
        enabled = true;
    }
}