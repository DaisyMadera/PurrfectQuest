using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawnmowerSpawner : MonoBehaviour
{
    public GameObject lawnmowerPrefab; 
    public Transform[] spawnPoints; 
    public bool lawnmowerSpawned = false;

    void Start()
    {
        SpawnLawnmower();
    }

    public void SpawnLawnmower()
    {
        // Ensure there are spawn points assigned
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned to LawnmowerSpawner.");
            return;
        }

        // Select a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnLocation = spawnPoints[randomIndex];

        // Spawn the LawnMower
        Instantiate(lawnmowerPrefab, spawnLocation.position, spawnLocation.rotation);
        lawnmowerSpawned = true;
    }
}
