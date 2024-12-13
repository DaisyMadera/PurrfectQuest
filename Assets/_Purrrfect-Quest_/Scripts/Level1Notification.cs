using UnityEngine;

public class Level1Notification : MonoBehaviour
{
    private void Start()
    {
        // Trigger a notification when this scene loads
        NotificationManager.ShowNotification("Welcome to Level 1!");
    }
}