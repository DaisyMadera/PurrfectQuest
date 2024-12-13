using UnityEngine;

public class Level2Initializer : MonoBehaviour
{
    Sprite mySprite;
    private void Start()
    {
        // Trigger a notification when this scene loads
        NotificationManager.ShowNotification("You got out!\nPlease collect all the notes to find the next clue to where your person is!");
    }
}