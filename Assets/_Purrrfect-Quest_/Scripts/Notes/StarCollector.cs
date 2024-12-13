using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarCollector : MonoBehaviour
{
    public int totalStars = 5; // Total number of stars to collect
    public int starsCollected = 0; // Current count of collected stars
    public TextMeshProUGUI starCounterText; // Reference to the HUD text
    public RoombaSpawner roombaSpawner;
    public watchdogSpawner watchdogSpawner;

    void Start()
    {
        UpdateStarCounter();
    }

    public void CollectStar()
    {
        starsCollected++;
        UpdateStarCounter();

        // Check if all stars are collected
        if (starsCollected >= totalStars)
        {
            Debug.Log("All Notes are collected!");
            if (roombaSpawner != null)
            {
                roombaSpawner.SpawnRoomba();
            }

            if (watchdogSpawner != null)
            {  
                watchdogSpawner.SpawnWatchdog(); 
            }
            
        }
    }

    void UpdateStarCounter()
    {
        starCounterText.text = "Collect all the Notes: " + starsCollected + "/" + totalStars;
    }
}
