using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class watchdogSpawner : MonoBehaviour
{
    public GameObject lawnmowerPrefab;
    public Transform[] spawnPoints;
    public bool WatchdogSpawned = false;

    void Start()
    {
    }

    public void SpawnWatchdog()
    {
        // Ensure there are spawn points assigned
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned to WatchdogSpawner.");
            return;
        }

        // Select a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnLocation = spawnPoints[randomIndex];

        // Spawn the Roomba
        Instantiate(lawnmowerPrefab, spawnLocation.position, spawnLocation.rotation);
        WatchdogSpawned = true;
    }
}
