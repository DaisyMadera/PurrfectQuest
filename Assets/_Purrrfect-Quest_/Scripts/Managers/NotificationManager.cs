using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public GameObject canvasPrefab; // Reference to the notification canvas prefab
    public GameObject textPrefab;   // Reference to the text prefab
    private static NotificationManager instance;

    [SerializeField] private float inputBlockTime = 5f; // Time to block input before allowing key press

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
        GameObject canvasInstance = Instantiate(canvasPrefab);
        GameObject textInstance = Instantiate(textPrefab, canvasInstance.transform);

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

        StartCoroutine(WaitForAnyKey(canvasInstance));
    }

    private IEnumerator WaitForAnyKey(GameObject canvasInstance)
    {
        Debug.Log($"Blocking input for {inputBlockTime} seconds...");
        yield return new WaitForSeconds(inputBlockTime); // Block input for the specified time

        Debug.Log("Now waiting for key press to close notification...");
        yield return new WaitUntil(() => Input.anyKeyDown); // Wait for a key press after the block period

        Destroy(canvasInstance);
        Debug.Log("Notification closed.");
    }
}
