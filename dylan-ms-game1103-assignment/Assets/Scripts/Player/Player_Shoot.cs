using System.Collections;
using UnityEngine;

/// <summary>
/// Handles player shooting mechanics, including ammo, reloading, and fire rate.
/// </summary>
[RequireComponent(typeof(Player_Input))]
public class Player_Shoot : MonoBehaviour
{
    [Header("Shooting")]
    [Tooltip("The projectile prefab to be instantiated.")]
    public GameObject projectilePrefab;
    [Tooltip("The point from which projectiles are fired.")]
    public Transform firePoint;
    [Tooltip("The time in seconds between shots.")]
    public float fireRate = 0.5f;

    [Header("Ammo & Reloading")]
    [Tooltip("The maximum number of projectiles in a clip.")]
    public int maxAmmo = 30;
    [Tooltip("The time in seconds it takes to reload.")]
    public float reloadTime = 2f;

    public int CurrentAmmo => currentAmmo;
    public bool IsReloading => isReloading;

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private Player_Input playerInput;

    void Awake()
    {
        playerInput = GetComponent<Player_Input>();
    }

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Use the AttackInput from our central input script.
        if (playerInput.AttackInput && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        currentAmmo--;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    /// <summary>
    /// Adds a specified amount of ammo to the player's current ammo count.
    /// </summary>
    /// <param name="amount">The amount of ammo to add.</param>
    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;
    }
}