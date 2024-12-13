using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarCollectorGarden : MonoBehaviour
{
    public int totalStars = 5; // Total number of stars to collect
    public int starsCollected = 0; // Current count of collected stars
    public TextMeshProUGUI starCounterText; // Reference to the HUD text
    private NotificationManager notificationManager;
    public RoombaSpawner roombaSpawner;
    public watchdogSpawner watchdogSpawner;

    private void Awake()
    {
        // Find and cache the NotificationManager reference
        notificationManager = FindObjectOfType<NotificationManager>();

        // Optional: Log an error if NotificationManager is not found
        if (notificationManager == null)
        {
            Debug.LogError("NotificationManager not found in the scene!");
        }
    }
    void Start()
    {
        UpdateStarCounter();
    }

    public void CollectStar()
    {
        starsCollected++;
        UpdateStarCounter();

        // Check if all stars are collected
        if (starsCollected >= totalStars && notificationManager != null)
        {
            Debug.Log("All Notes are collected!");
            NotificationManager.ShowNotification("You have collected all the notes!\nBe careful, the dog is on the way!");
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
        starCounterText.text = starsCollected + "/" + totalStars;
    }
}
