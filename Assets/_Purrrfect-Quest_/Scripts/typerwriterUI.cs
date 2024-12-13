using System.Collections;
using UnityEngine;
using TMPro;

public class typewriterUI : MonoBehaviour
{
    private TMP_Text _tmpProText;
    private string writer;

    [SerializeField] private float delayBeforeStart = 0f;
    [SerializeField] private float timeBtwChars = 0.1f;
    [SerializeField] private string leadingChar = "";
    [SerializeField] private bool leadingCharBeforeDelay = false;

    private void Awake()
    {
        _tmpProText = GetComponent<TMP_Text>();
        if (_tmpProText == null)
        {
            Debug.LogError("TMP_Text component not found!");
        }
    }

    public void StartTypewriter(string message)
    {
        writer = message;
        if (_tmpProText != null)
        {
            _tmpProText.text = "";
            StartCoroutine(TypeWriterTMP());
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
    }
}
