using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Access the StarCollector script
            StarCollector starCollector = FindObjectOfType<StarCollector>();
            if (starCollector != null)
            {
                starCollector.CollectStar();
            }

            // Destroy this star object
            Destroy(gameObject);
        }
    }
}
