using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1f;
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
