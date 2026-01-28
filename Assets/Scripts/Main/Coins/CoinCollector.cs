using System;
using Unity.Netcode;
using UnityEngine;

public class CoinCollector : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private BountyCoin coinPrefab;
    [Header("Settings")]
    [SerializeField] private float bountyPercentage = 50;
    [SerializeField] private float coinSpread = 3;
    [SerializeField] private int bountyCoinCount = 10;
    [SerializeField] private int minBountyCoinValue = 5;
    [SerializeField] private ContactFilter2D layerMask;
    private Collider2D[] coinBuffer = new Collider2D[1];

    private float coinRadius;
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;
        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;
        health.OnDie += HandlePlayerDeath;
    }

    public override void OnNetworkDespawn()
    {
        if(!IsServer) return;
        health.OnDie -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath(Health health)
    {
        int bountyValue = (int) (TotalCoins.Value * (bountyPercentage/100f));
        int bountyCoinValue = bountyValue / bountyCoinCount;
        if(bountyCoinValue < minBountyCoinValue) return;
        for(int i=0;i<bountyCoinCount;i++)
        {
            BountyCoin coinInstance = Instantiate(coinPrefab, GetSpawnPoint(), Quaternion.identity);
            coinInstance.SetValue(bountyCoinValue);
            coinInstance.NetworkObject.Spawn();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out Coin coin))
        {
            int value = coin.Collect();
            if(!IsServer) return;
            TotalCoins.Value+=value;
        }
    }

    private Vector2 GetSpawnPoint()
    {
        while(true)
        {
            Vector2 spawnPoint = (Vector2) transform.position + UnityEngine.Random.insideUnitCircle * coinSpread;
            int numColliders = Physics2D.OverlapCircle(spawnPoint, coinRadius, layerMask, coinBuffer);
            if(numColliders==0)
            {
                return spawnPoint;
            }
        }
    }

    public void SpendCoins(int coins)
    {
        TotalCoins.Value -= coins;
    }
}
