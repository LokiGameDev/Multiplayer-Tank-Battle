using System;
using Unity.Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private SpriteRenderer playerMinimapIcon;
    [Header("Settings")]
    [SerializeField] private int ownerPriority = 15;
    [field: SerializeField] public Health Health { get; private set;}
    [field: SerializeField] public CoinCollector Wallet { get; private set;}
    [SerializeField] private Color ownPlayerIconColor;

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    public static event Action<TankPlayer> OnPlayerSpawned;
    public static event Action<TankPlayer> OnPlayerDespawned;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            UserData data = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
            PlayerName.Value = data.userName;
            OnPlayerSpawned?.Invoke(this);
        }
        if(IsOwner)
        {
            cinemachineCamera.Priority = ownerPriority;
            playerMinimapIcon.color = ownPlayerIconColor;
        }
    }

    public override void OnNetworkDespawn()
    {
        if(IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }
}
