using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected int coinValue = 10;
    protected bool alreadyCollected;
    public abstract int Collect();
    public void SetValue(int value)
    {
        coinValue = value;
    }
    protected void Show(bool status)
    {
        spriteRenderer.enabled = status;
    }
    protected void BountyShow(bool status)
    {
        Destroy(gameObject, 0.1f);
    }
}
