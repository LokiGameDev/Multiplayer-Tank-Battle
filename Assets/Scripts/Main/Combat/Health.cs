using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int MaxHealth {get; private set;} = 100;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
    [SerializeField] private GameObject deathEffect;

    private bool isDead;

    public Action<Health> OnDie;

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;

        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
        GetComponent<TankPlayer>().PlayHealSFX();
    }

    private void ModifyHealth(int value)
    {
        if(isDead) return;
        CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value+value, 0, MaxHealth);
        if(CurrentHealth.Value <= 0)
        {
            isDead = true;
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            OnDie?.Invoke(this);
        }
    }
}
