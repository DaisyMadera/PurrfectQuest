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
            Debug.Log("Starting typewriter effect with optional sprite...");
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
        yield return new WaitUntil(() => Input.anyKeyDown);
        Destroy(canvasInstance);
        Debug.Log("Notification closed.");
    }
}
