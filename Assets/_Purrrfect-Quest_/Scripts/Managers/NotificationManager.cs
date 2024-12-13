using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public GameObject canvasPrefab; // Reference to the notification canvas prefab
    public GameObject textPrefab;   // Reference to the text prefab
    private static NotificationManager instance;

    [SerializeField] private float inputBlockTime = 5f; // Time to block input before allowing key press
    private GameObject currentCanvasInstance; // Keep track of the active notification
    private bool isPausedForNotification = false; // To prevent multiple notifications from pausing

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ShowNotification(string message, Sprite sprite = null)
    {
        if (instance == null)
        {
            Debug.LogError("NotificationManager not found in the scene!");
            return;
        }

        instance.DisplayNotification(message, sprite);
    }

    private void DisplayNotification(string message, Sprite sprite)
    {
        if (currentCanvasInstance != null)
        {
            Debug.LogWarning("A notification is already active. Ignoring new notification.");
            return; // Prevent overlapping notifications
        }

        currentCanvasInstance = Instantiate(canvasPrefab);
        GameObject textInstance = Instantiate(textPrefab, currentCanvasInstance.transform);

        typewriterUI typewriter = textInstance.GetComponent<typewriterUI>();
        if (typewriter != null)
        {
            Debug.Log($"Sprite passed to NotificationManager: {sprite}");
            typewriter.StartTypewriter(message, sprite);
        }
        else
        {
            TMP_Text notificationText = textInstance.GetComponent<TMP_Text>();
            if (notificationText != null)
            {
                notificationText.text = message;
            }
        }

        PauseGame(); // Pause the game when the notification is displayed
        StartCoroutine(WaitForAnyKey());
    }

    private IEnumerator WaitForAnyKey()
    {
        Debug.Log($"Blocking input for {inputBlockTime} seconds...");
        yield return new WaitForSeconds(inputBlockTime); // Block input for the specified time

        Debug.Log("Now waiting for key press to close notification...");
        yield return new WaitUntil(() => Input.anyKeyDown); // Wait for a key press after the block period

        Destroy(currentCanvasInstance); // Close the notification
        currentCanvasInstance = null;

        ResumeGame(); // Resume the game after the notification is closed
        Debug.Log("Notification closed.");
    }

    private void PauseGame()
    {
        if (!isPausedForNotification)
        {
            Time.timeScale = 0f; // Pause the game
            isPausedForNotification = true;
            Debug.Log("Game paused for notification.");
        }
    }

    private void ResumeGame()
    {
        if (isPausedForNotification)
        {
            Time.timeScale = 1f; // Resume the game
            isPausedForNotification = false;
            Debug.Log("Game resumed after notification.");
        }
    }
}
