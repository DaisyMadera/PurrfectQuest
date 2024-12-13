using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class typewriterUI : MonoBehaviour
{
    private TMP_Text _tmpProText;
    private string writer;

    [SerializeField] private float delayBeforeStart = 0f;
    [SerializeField] private float timeBtwChars = 0.1f;
    [SerializeField] private string leadingChar = "";
    [SerializeField] private bool leadingCharBeforeDelay = false;

    [SerializeField] private Image spriteImage; // UI Image to display the sprite
    [SerializeField] private Sprite optionalSprite; // Assign in the prefab if you want a default sprite

    private bool isTyping = false; // Flag to check if typing is ongoing
    private bool isWaitingForKey = false; // Flag to check if waiting for key press to close notification
    private bool skipTyping = false; // Flag to skip typing

    private void Awake()
    {
        _tmpProText = GetComponent<TMP_Text>();
        if (_tmpProText == null)
        {
            Debug.LogError("TMP_Text component not found!");
        }

        if (spriteImage != null)
        {
            spriteImage.enabled = false; // Ensure the sprite is hidden initially
        }
    }

    public void StartTypewriter(string message, Sprite sprite = null)
    {
        Debug.Log($"Received message for typewriter: {message}\nSprite passed: {sprite}");
        writer = message;
        skipTyping = false;

        if (spriteImage != null)
        {
            Sprite finalSprite = sprite != null ? sprite : optionalSprite;
            if (finalSprite != null)
            {
                spriteImage.sprite = finalSprite;
                spriteImage.enabled = false;
            }
            else
            {
                Debug.Log("No sprite provided or assigned as optionalSprite.");
            }
        }

        if (_tmpProText != null)
        {
            _tmpProText.text = ""; // Clear any existing text
            StartCoroutine(TypeWriterTMP());
        }
        else
        {
            Debug.LogError("TMP_Text component is null. Cannot run typewriter effect.");
        }
    }

    private IEnumerator TypeWriterTMP()
    {
        isTyping = true;
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSecondsRealtime(delayBeforeStart);

        foreach (char c in writer)
        {
            if (skipTyping)
            {
                _tmpProText.text = writer; // Skip to the full text
                break;
            }

            _tmpProText.text += c;
            _tmpProText.text += leadingChar;
            yield return new WaitForSecondsRealtime(timeBtwChars);
        }

        if (leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
        }

        isTyping = false;
        isWaitingForKey = true; // Start waiting for key press
        Debug.Log("Typewriter effect completed.");

        // Display the sprite if available
        if (spriteImage != null && spriteImage.sprite != null)
        {
            spriteImage.enabled = true;
        }
        else
        {
            Debug.Log("No sprite to display after typewriter.");
        }
    }

    private void Update()
    {
        if (isTyping && Input.anyKeyDown)
        {
            // Skip typing and immediately display the full text
            skipTyping = true;
        }
        else if (isWaitingForKey && Input.anyKeyDown)
        {
            // Close the notification and resume gameplay
            CloseNotification();
        }
    }

    private void CloseNotification()
    {
        Debug.Log("Notification closed. Resuming gameplay.");
        isWaitingForKey = false;

        // Destroy the notification GameObject
        Destroy(gameObject);

        // Resume gameplay if paused
        Time.timeScale = 1f;
    }
}
