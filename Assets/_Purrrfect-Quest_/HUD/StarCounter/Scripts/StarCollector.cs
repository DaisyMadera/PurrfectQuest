using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarCollector : MonoBehaviour
{
    public int totalStars = 5; // Total number of stars to collect
    public int starsCollected = 0; // Current count of collected stars
    public TextMeshProUGUI starCounterText; // Reference to the HUD text

    void Start()
    {
        UpdateStarCounter();
    }

    public void CollectStar()
    {
        starsCollected++;
        UpdateStarCounter();

        // Check if all stars are collected
        if (starsCollected >= totalStars)
        {
            Debug.Log("All stars collected!");
            // we should have win condition or anything here
        }
    }

    void UpdateStarCounter()
    {
        starCounterText.text = "Stars: " + starsCollected + "/" + totalStars;
    }
}
