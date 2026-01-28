using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject coinPrefab;
    [Header("Settings")]
    [SerializeField] private int maxCoins;
    [SerializeField] private int coinValue;
    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private ContactFilter2D layerMask;

    private Collider2D[] coinBuffer = new Collider2D[1];

    private float coinRadius;

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;
        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;
        for(int i=0;i<maxCoins;i++)
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        RespawningCoin coinInstance = Instantiate(coinPrefab, GetSpawnPoint(), Quaternion.identity, gameObject.transform).GetComponent<RespawningCoin>();

        coinInstance.SetValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();

        coinInstance.OnCollect += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawningCoin coin)
    {
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
    }

    private Vector2 GetSpawnPoint()
    {
        float x = 0;
        float y = 0;
        while(true)
        {
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            Vector2 spawnPoint = new Vector2(x,y);
            int numColliders = Physics2D.OverlapCircle(spawnPoint, coinRadius, layerMask, coinBuffer);
            if(numColliders==0)
            {
                return spawnPoint;
            }
        }
    }
}
