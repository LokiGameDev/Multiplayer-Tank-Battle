using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private TankPlayer playerPrefab;
    [SerializeField] private float keptCoinPercentage = 50f;

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;

        TankPlayer[] tankPlayers = FindObjectsByType<TankPlayer>(FindObjectsSortMode.None);

        foreach(TankPlayer player in tankPlayers)
        {
            HandlePlayerSpawned(player);
        }

        TankPlayer.OnPlayerSpawned += HandlePlayerSpawned;
        TankPlayer.OnPlayerDespawned += HandlePlayerDespawned;
    }

    public override void OnNetworkDespawn()
    {
        if(!IsServer) return;

        TankPlayer.OnPlayerSpawned -= HandlePlayerSpawned;
        TankPlayer.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(TankPlayer player)
    {
        player.Health.OnDie += (health) => HandlePlayerDeath(player);
    }

    private void HandlePlayerDespawned(TankPlayer player)
    {
        player.Health.OnDie -= (health) => HandlePlayerDeath(player);
    }

    private void HandlePlayerDeath(TankPlayer player)
    {
        int remainingCoin = (int) (player.Wallet.TotalCoins.Value * (keptCoinPercentage/100));
        Destroy(player.gameObject);
        StartCoroutine(RespawnPlayer(player.OwnerClientId, remainingCoin));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId, int remainingCoin)
    {
        yield return new WaitForSeconds(0.1f);
        TankPlayer playerInstance = Instantiate(playerPrefab, SpawnPoint.GetRandomSpawnPoint(), Quaternion.identity);
        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
        playerInstance.Wallet.TotalCoins.Value = remainingCoin;
    }
}
