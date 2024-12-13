using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public GameObject canvasPrefab; // Reference to the notification canvas prefab
    public GameObject textPrefab;   // Reference to the text prefab

    private static NotificationManager instance;

    private void Awake()
    {
        // Singleton pattern to ensure one instance persists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Avoid duplicates
        }
    }

    public static void ShowNotification(string message)
    {
        if (instance == null)
        {
            Debug.LogError("NotificationManager not found in the scene!");
            return;
        }

        instance.DisplayNotification(message);
    }

    private void DisplayNotification(string message)
    {
        // Instantiate the canvas prefab
        GameObject canvasInstance = Instantiate(canvasPrefab);

        // Instantiate the text prefab as a child of the canvas
        GameObject textInstance = Instantiate(textPrefab, canvasInstance.transform);

        // Set the message on the TMP_Text component
        TMP_Text notificationText = textInstance.GetComponent<TMP_Text>();
        if (notificationText != null)
        {
            notificationText.text = message; // Set the notification text
        }

        // Start coroutine to wait for any key press
        StartCoroutine(WaitForAnyKey(canvasInstance));
    }

    private IEnumerator WaitForAnyKey(GameObject canvasInstance)
    {
        // Wait until any key is pressed
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Destroy the notification canvas
        Destroy(canvasInstance);
    }
}
