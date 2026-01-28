using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text playerPositionText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerCoinsText;
    [SerializeField] private Color myColor;

    private FixedString32Bytes playerName;
    public ulong ClientId {get; private set;}
    public int Coins {get; private set;}

    public void Initialize(ulong clientId, FixedString32Bytes playerName, int coins)
    {
        ClientId = clientId;
        this.playerName = playerName;
        Coins = coins;

        if(clientId == NetworkManager.Singleton.LocalClientId)
        {
            playerCoinsText.color = myColor;
            playerNameText.color = myColor;
            playerPositionText.color = myColor;
        }

        UpdateText();
    }

    public void UpdateCoins(int coins)
    {
        Coins = coins;

        playerCoinsText.text = Coins.ToString();
    }

    internal void UpdatePosition()
    {
        playerPositionText.text = $"{transform.GetSiblingIndex() + 1}.";
    }

    private void UpdateText()
    {
        playerNameText.text = playerName.ToString();
        playerCoinsText.text = Coins.ToString();
    }
}
