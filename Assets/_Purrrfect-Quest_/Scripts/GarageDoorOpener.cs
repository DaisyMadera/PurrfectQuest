using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageDoorOpener : MonoBehaviour
{
    private NotificationManager notificationManager;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ensure collision is with the Player and notificationManager is not null
        if (collision.CompareTag("Player") && notificationManager != null)
        {
            NotificationManager.ShowNotification("The Garage Door Has Been Opened!");
        }
    }
}
