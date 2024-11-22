using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour, IDamageable
{
    public float maxHealth = 3f;
    private float currentHealth;

    public Canvas healthBarCanvas;
    public Image healthBarImage;

    private GameManager gameManager;

    void Start()
    {
        currentHealth = maxHealth;
        gameManager = GameObject.FindObjectOfType<GameManager>();

        if (healthBarImage == null)
        {
            Debug.LogError("No Image component found on the healthBarCanvas. Please add an Image component to the Canvas.");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            gameManager.BaseDestroyed();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }
}
