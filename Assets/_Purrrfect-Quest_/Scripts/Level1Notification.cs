using UnityEngine;

public class Level1Notification : MonoBehaviour
{
    Sprite mySprite;
    private void Start()
    {
        mySprite = Resources.Load<Sprite>("Assets/_Purrrfect-Quest_/Sprites/PowerUps/Letter.png");
        // Trigger a notification when this scene loads
        NotificationManager.ShowNotification("Goodmorning!\nPlease collect all the notes to find the first clue to where your person is", mySprite);
    }
}