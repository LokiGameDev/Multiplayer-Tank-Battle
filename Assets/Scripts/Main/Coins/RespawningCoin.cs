using System;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollect;
    private Vector3 previousPosition;

    void Update()
    {
        if(previousPosition!=transform.position)
        {
            Show(true);
        }
        previousPosition = transform.position;
    }
    public override int Collect()
    {
        if(!IsServer)
        {
            Show(false);
            return 0;
        }
        if(alreadyCollected) { return 0; }

        alreadyCollected = true;
        OnCollect?.Invoke(this);

        return coinValue;
    }

    internal void Reset()
    {
        alreadyCollected = false;
    }
}
