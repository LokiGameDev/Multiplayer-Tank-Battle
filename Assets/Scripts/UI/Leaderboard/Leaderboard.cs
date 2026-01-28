using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Leaderboard : NetworkBehaviour
{
    [SerializeField] private Transform leaderboardEntityHolder;
    [SerializeField] private LeaderboardItem leaderboardItemPrefab;
    [SerializeField] private int itemsToDisplay = 8;

    private NetworkList<LeaderboardItemState> leaderboardItemStates;
    private List<LeaderboardItem> leaderboardItems = new List<LeaderboardItem>();

    private void Awake()
    {
        leaderboardItemStates = new NetworkList<LeaderboardItemState>();
    }

    public override void OnNetworkSpawn()
    {
        if(IsClient)
        {
            leaderboardItemStates.OnListChanged += HandleLeaderboardEntitiesChanged;
            foreach(LeaderboardItemState item in leaderboardItemStates)
            {
                HandleLeaderboardEntitiesChanged(new NetworkListEvent<LeaderboardItemState>
                {
                    Type = NetworkListEvent<LeaderboardItemState>.EventType.Add,
                    Value = item
                });
            }
        }
        if(IsServer)
        {
            TankPlayer[] tankPlayers = FindObjectsByType<TankPlayer>(FindObjectsSortMode.None);

            foreach(TankPlayer player in tankPlayers)
            {
                HandlePlayerSpawned(player);
            }

            TankPlayer.OnPlayerSpawned += HandlePlayerSpawned;
            TankPlayer.OnPlayerDespawned += HandlePlayerDespawned;
        }
    }

    private void HandleLeaderboardEntitiesChanged(NetworkListEvent<LeaderboardItemState> changeEvent)
    {
        switch(changeEvent.Type)
        {
            case NetworkListEvent<LeaderboardItemState>.EventType.Add:
                if(!leaderboardItems.Any(x => x.ClientId == changeEvent.Value.ClientId))
                {
                    if(leaderboardItemPrefab==null) break;
                    LeaderboardItem item = Instantiate(leaderboardItemPrefab, leaderboardEntityHolder);
                    item.Initialize(changeEvent.Value.ClientId, changeEvent.Value.PlayerName, changeEvent.Value.Coins);
                    leaderboardItems.Add(item);
                }
                break;
            case NetworkListEvent<LeaderboardItemState>.EventType.Remove:
                LeaderboardItem removeItem = leaderboardItems.FirstOrDefault(x => x.ClientId == changeEvent.Value.ClientId);
                if(removeItem!=null)
                {
                    removeItem.transform.SetParent(null);
                    Destroy(removeItem.gameObject);
                    leaderboardItems.Remove(removeItem);
                }
                break;
            case NetworkListEvent<LeaderboardItemState>.EventType.Value:
                LeaderboardItem updateItem = leaderboardItems.FirstOrDefault(x => x.ClientId == changeEvent.Value.ClientId);
                if(updateItem!=null)
                {
                    updateItem.UpdateCoins(changeEvent.Value.Coins);
                }
                break;
        }

        leaderboardItems.Sort((x, y) => y.Coins.CompareTo(x.Coins));

        for(int i=0;i<leaderboardItems.Count;i++)
        {
            leaderboardItems[i].transform.SetSiblingIndex(i);
            leaderboardItems[i].UpdatePosition();
            leaderboardItems[i].gameObject.SetActive(i <= (itemsToDisplay-1));
        }

        LeaderboardItem myItem = leaderboardItems.FirstOrDefault(x => x.ClientId == NetworkManager.Singleton.LocalClientId);

        if(myItem != null)
        {
            if(myItem.transform.GetSiblingIndex() >= itemsToDisplay)
            {
                leaderboardEntityHolder.GetChild(itemsToDisplay - 1).gameObject.SetActive(false);
                myItem.gameObject.SetActive(true);
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        if(IsClient)
        {
            leaderboardItemStates.OnListChanged += HandleLeaderboardEntitiesChanged;
        }
        if(IsServer)
        {    
            TankPlayer.OnPlayerSpawned -= HandlePlayerSpawned;
            TankPlayer.OnPlayerDespawned -= HandlePlayerDespawned;
        }
    }

    private void HandlePlayerSpawned(TankPlayer player)
    {
        leaderboardItemStates.Add(new LeaderboardItemState
        {
            ClientId = player.OwnerClientId,
            PlayerName = player.PlayerName.Value,
            Coins = 0 
        });

        player.Wallet.TotalCoins.OnValueChanged += (oldCoins, newCoins) => HandleCoinsChanged(player.OwnerClientId, newCoins);
    }

    private void HandlePlayerDespawned(TankPlayer player)
    {
        if(leaderboardItemStates == null) return;

        foreach(LeaderboardItemState item in leaderboardItemStates)
        {
            if(item.ClientId != player.OwnerClientId) continue;

            leaderboardItemStates.Remove(item);
            break;
        }

        player.Wallet.TotalCoins.OnValueChanged -= (oldCoins, newCoins) => HandleCoinsChanged(player.OwnerClientId, newCoins);
    }

    private void HandleCoinsChanged(ulong clientId, int newCoins)
    {
        for(int i=0;i<leaderboardItemStates.Count;i++)
        {
            if(leaderboardItemStates[i].ClientId != clientId) continue;

            leaderboardItemStates[i] = new LeaderboardItemState
            {
                ClientId = leaderboardItemStates[i].ClientId,
                PlayerName = leaderboardItemStates[i].PlayerName,
                Coins = newCoins
            };

            return;
        }
    }
}
