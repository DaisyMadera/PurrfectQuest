using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaSpawner : MonoBehaviour
{
    public GameObject roombaPrefab; // Assign your Roomba prefab
    public Transform[] spawnPoints; // Assign spawn points in the Inspector
    public bool roombaSpawned = false;

    void Start()
    {
        // SpawnRoomba();
    }

    public void SpawnRoomba()
    {
        // Ensure there are spawn points assigned
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned to RoombaSpawner.");
            return;
        }

        // Select a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnLocation = spawnPoints[randomIndex];

        // Spawn the Roomba
        Instantiate(roombaPrefab, spawnLocation.position, spawnLocation.rotation);
        roombaSpawned = true;
    }
}
