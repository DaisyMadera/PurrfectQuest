using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageDoor : MonoBehaviour
{
    [SerializeField] private Collider2D blockingCollider; // Reference to the blocking collider

    private void Start()
    {
        // Ensure the blocking collider is active initially
        if (blockingCollider != null)
        {
            blockingCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with the Player
        if (collision.CompareTag("Player"))
        {
            // Check if the opener has been collected
            if (GarageDoorOpener.openerCollected)
            {
                NotificationManager.ShowNotification("You are now entering the Garden...");
                AllowPassage(); // Disable the blocking collider
                LoadLevel2();
            }
            else
            {
                NotificationManager.ShowNotification("You need the garage door opener to leave!");
            }
        }
    }

    private void AllowPassage()
    {
        // Disable the blocking collider
        if (blockingCollider != null)
        {
            blockingCollider.enabled = false;
        }
    }

    private void LoadLevel2()
    {
        // Load the next level 
        SceneManager.LoadScene("Garden"); 
    }
}
