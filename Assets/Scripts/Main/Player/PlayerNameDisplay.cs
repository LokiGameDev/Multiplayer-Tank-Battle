using System;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private TankPlayer tankPlayer;
    [SerializeField] private TMP_Text playerNameText;
    void Start()
    {
        HandlePlayerNameChanged(string.Empty, tankPlayer.PlayerName.Value);
        
        tankPlayer.PlayerName.OnValueChanged += HandlePlayerNameChanged;
    }

    private void HandlePlayerNameChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        playerNameText.text = newValue.ToString();
    }

    private void OnDestroy()
    {
        tankPlayer.PlayerName.OnValueChanged -= HandlePlayerNameChanged;
    }
}
