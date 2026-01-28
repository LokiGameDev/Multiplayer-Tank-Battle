using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    void OnEnable()
    {
        spawnPoints.Add(this);
    }

    public static Vector3 GetRandomSpawnPoint()
    {
        if(spawnPoints.Count == 0)
        {
            return Vector3.zero;
        }
        return spawnPoints[Random.Range(0,spawnPoints.Count)].transform.position;
    }

    void OnDisable()
    {
        spawnPoints.Remove(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
