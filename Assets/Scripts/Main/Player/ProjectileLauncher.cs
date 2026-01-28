using System;
using System.Collections;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private CoinCollector collector;
    [SerializeField] private Image ammoBarImage;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float muzzleFlashDuration;
    [SerializeField] private int coinToFire;

    private bool shouldFire;
    private float timer;
    private float muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
        inputReader.MobileFireEvent += HandleMobileFire;
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner) return;
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
        inputReader.MobileFireEvent -= HandleMobileFire;
    }

    void Update()
    {
        if(muzzleFlashTimer>0)
        {
            muzzleFlashTimer-=Time.deltaTime;
            if(muzzleFlashTimer <= 0)
            {
                muzzleFlash.SetActive(false);
            }
        }

        if(!IsOwner) return;

        if(timer>0)
        {
            timer -= Time.deltaTime;
            ammoBarImage.fillAmount = ((1/fireRate)-timer)/(1/fireRate);
        }

        if(!shouldFire) return;

        if(timer > 0) return;

        if(collector.TotalCoins.Value < coinToFire) return;

        PrimaryFireServerRPC(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
        timer = 1/fireRate;
        ammoBarImage.fillAmount = ((1/fireRate)-timer)/(1/fireRate);
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }

    private void HandleMobileFire()
    {
        shouldFire = true;
        StartCoroutine(StopMobileShoot());
    }

    IEnumerator StopMobileShoot()
    {
        yield return new WaitForSeconds(0.2f);
        shouldFire = false;
    }

    [ServerRpc]
    private void PrimaryFireServerRPC(Vector3 spawnPos, Vector3 direction)
    {
        if(collector.TotalCoins.Value < coinToFire) return;

        collector.SpendCoins(coinToFire);

        GameObject projectileInstance = Instantiate(serverProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.up = direction;

        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if(projectileInstance.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.transform.up * projectileSpeed;
        }

        SpawnDummyProjectileClientRPC(spawnPos, direction);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRPC(Vector3 spawnPos, Vector3 direction)
    {
        if(IsOwner) return;
        SpawnDummyProjectile(spawnPos, direction);
    }

    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;
        GameObject projectileInstance = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.up = direction;
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());
        if(projectileInstance.TryGetComponent(out DealDamageOnContact dealDamageOnContact))
        {
            dealDamageOnContact.SetOwner(OwnerClientId);
        }

        if(projectileInstance.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = rb.transform.up * projectileSpeed;
        }
    }
}
