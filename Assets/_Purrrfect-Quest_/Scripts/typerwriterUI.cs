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
    [SerializeField] private Animator spriteAnimator; // Animator for sprite animation

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
        Debug.Log($"Received message for typewriter: {message}"); // Debugging line
        writer = message;

        if (_tmpProText != null)
        {
            _tmpProText.text = ""; // Clear any existing text
            StartCoroutine(TypeWriterTMP(sprite));
        }
        else
        {
            Debug.LogError("TMP_Text component is null. Cannot run typewriter effect.");
        }
    }

    private IEnumerator TypeWriterTMP(Sprite sprite)
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

        // Handle the sprite display
        if (spriteImage != null)
        {
            if (sprite != null)
            {
                spriteImage.sprite = sprite;
                spriteImage.enabled = true;

                // Trigger animation if animator exists
                if (spriteAnimator != null)
                {
                    spriteAnimator.SetTrigger("PlayAnimation"); // Ensure you have a trigger in your Animator
                }
            }
            else
            {
                Debug.Log("No sprite provided for this notification.");
            }
        }
    }
}
