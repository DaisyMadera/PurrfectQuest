using System.Collections;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public WatchdogController watchdogController;  // Reference to the boss (WatchdogController)
    
    private bool gameWon = false;  // To check if the game is already won

    void Update()
    {
        // Continuously check the boss's health
        if (watchdogController != null && !gameWon)
        {
            // Use the getter to check if the boss is defeated
            if (watchdogController.GetCurrentHealth() <= 0)
            {
                // Boss is defeated, show the "You Win!" notification
                NotificationManager.ShowNotification("Thank You for Playing!");
                gameWon = true;  // Prevent the notification from showing again
            }
        }
    }
}