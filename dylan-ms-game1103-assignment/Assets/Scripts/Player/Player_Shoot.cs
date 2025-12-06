using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player_Input))]
public class Player_Shoot : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;

    [Header("Ammo & Reloading")]
    public int maxAmmo = 30;
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

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;
    }
}