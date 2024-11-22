using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    public GameObject basePrefab; // Assign your Base prefab
    public Transform[] baseSpawnPoints; // Assign spawn points in the Inspector
    public int basesToSpawn = 3; // Number of bases to spawn

    void Start()
    {
        SpawnBases();
    }

    void SpawnBases()
    {
        // Create a list of available spawn points
        List<Transform> availableSpawnPoints = new List<Transform>(baseSpawnPoints);

        for (int i = 0; i < basesToSpawn; i++)
        {
            if (availableSpawnPoints.Count == 0)
                break;

            // Select a random spawn point
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnLocation = availableSpawnPoints[randomIndex];

            // Spawn the base
            Instantiate(basePrefab, spawnLocation.position, spawnLocation.rotation);

            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }
}
