using System.Collections;
using UnityEngine;

public class GarageDoorOpener : MonoBehaviour
{
    private NotificationManager notificationManager;

    public static bool openerCollected = false; // Static flag to track if the opener is collected

    private void Awake()
    {
        // Find and cache the NotificationManager reference
        notificationManager = FindObjectOfType<NotificationManager>();

        if (notificationManager == null)
        {
            Debug.LogError("NotificationManager not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ensure collision is with the Player
        if (collision.CompareTag("Player"))
        {
            NotificationManager.ShowNotification("You can now leave through the garage doors!");
            openerCollected = true; // Mark the opener as collected
            Destroy(gameObject); // Destroy the opener object
        }
    }
}
