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
    private Animator anim;
    RoombaSpawner roombaSpawner;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        if (healthBarImage == null)
        {
            Debug.LogError("No Image component found on the healthBarCanvas. Please add an Image component to the Canvas.");
        }
    }

    public void TakeDamage(float damage)
    {
        if (roombaSpawner != null && !roombaSpawner.roombaSpawned)
        {
            Debug.Log("Base cannot take damage until the Roomba is spawned.");
            return;
        }

        anim.SetBool("isHurt", true);
        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            anim.SetTrigger("Die");
            Destroy(gameObject);
            gameManager.BaseDestroyed();
        }
        anim.SetBool("isHurt", false);
    }

    void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }
}
