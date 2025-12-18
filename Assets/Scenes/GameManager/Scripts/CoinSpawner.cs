// Assets/Scripts/CoinSpawner.cs
using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public List<GameObject> coinPrefabs;
    public List<Transform> spawnPoints;

    void Start()
    {
        if (coinPrefabs?.Count > 0 && spawnPoints?.Count > 0)
        {
            foreach (var point in spawnPoints)
            {
                int index = Random.Range(0, coinPrefabs.Count);
                Instantiate(coinPrefabs[index], point.position, Quaternion.identity);
            }
        }
    }
}