using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    public Canvas healthBarCanvas; // Assign the Canvas in the Inspector
    private Image healthBar;

    private GameManager gameManager;

    void Start()
    {
        currentHealth = maxHealth;
        gameManager = GameObject.FindObjectOfType<GameManager>();

        // Get the Image component from the Canvas
        healthBar = healthBarCanvas.GetComponent<Image>();
        if (healthBar == null)
        {
            Debug.LogError("No Image component found on the healthBarCanvas. Please add an Image component to the Canvas.");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            gameManager.BaseDestroyed();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}
