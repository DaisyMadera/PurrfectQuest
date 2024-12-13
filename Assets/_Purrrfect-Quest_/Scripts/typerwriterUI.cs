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
    // [SerializeField] private Animator spriteAnimator; // Animator for sprite animation
    [SerializeField] private Sprite optionalSprite; // Assign in the prefab if you want a default sprite

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
        Debug.Log($"Received message for typewriter: {message}\nSprite passed: {sprite}"); // Debugging line
        writer = message;

        // Use the provided sprite, or fallback to the optional sprite
        if (spriteImage != null)
        {
            Sprite finalSprite = sprite != null ? sprite : optionalSprite;
            if (finalSprite != null)
            {
                Debug.Log($"Setting spriteImage.sprite to {finalSprite.name}");
                spriteImage.sprite = finalSprite;
                spriteImage.enabled = false; // Initially hidden; enabled later
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
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char c in writer)
        {
            if (_tmpProText.text.Length > 0)
            {
                _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
            }
            _tmpProText.text += c;
            _tmpProText.text += leadingChar;
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
        }

        Debug.Log("Typewriter effect completed.");

        // Display and animate the sprite if available
        if (spriteImage != null && spriteImage.sprite != null)
        {
            spriteImage.enabled = true;

            // // Trigger animation if animator exists
            // if (spriteAnimator != null)
            // {
            //     spriteAnimator.SetTrigger("PlayAnimation"); // Ensure a trigger exists in the Animator
            // }
        }
        else
        {
            Debug.Log("No sprite to display after typewriter.");
        }
    }
}
