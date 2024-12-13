using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoombaController : MonoBehaviour, IDamageable
{
    public float detectionRange = 5f;
    public float chaseSpeed = 3f;
    public float wanderSpeed = 1.5f;
    public float maxHealth = 100f;
    public float damageToPlayer = 10f;
    public float bounceForce = 1000f;

    private float currentHealth;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool canTakeDamage = false;
    private GameManager gameManager;

    public Canvas healthBarCanvas; // Assign the Canvas in the Inspector
    public Image healthBarImage;

    // Damage cooldown variables
    private bool canDamagePlayer = true;
    public float damageCooldown = 3f;
    private Animator anim;

    public GameObject dropItemPrefab;


    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Find the Player object by tag "Player"
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found in the scene. Please ensure the Player object has the tag 'Player'.");
        }

        gameManager = GameObject.FindObjectOfType<GameManager>();

        if (healthBarImage == null)
        {
            Debug.LogError("No Image component found on the healthBarCanvas. Please add an Image component to the Canvas.");
        }

        // Hide health bar initially
        healthBarCanvas.gameObject.SetActive(false);

        // Start wandering
        InvokeRepeating("SetRandomDirection", 0f, 1f); // Change direction every 2 seconds
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Chase the player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            movement = direction * chaseSpeed;
        }
        else
        {
            // Wander randomly
            movement = movement.normalized * wanderSpeed;
        }
        // Update Animator parameters
        anim.SetFloat("X", movement.x);
        anim.SetFloat("Y", movement.y);

        // Check if all bases are destroyed
        if (!canTakeDamage && gameManager.AllBasesDestroyed())
        {
            canTakeDamage = true;
            healthBarCanvas.gameObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }

    void SetRandomDirection()
    {
        if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) > detectionRange)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            movement = direction * wanderSpeed;
        }
    }

    // Detect collision with the Player to deal damage and bounce off
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (canDamagePlayer)
            {
                // Deal damage to the player
                collision.collider.GetComponent<CharacterHealth>().TakeDamage(damageToPlayer);

                // Start damage cooldown
                canDamagePlayer = false;
                StartCoroutine(DamageCooldown());

                // Bounce off the player
                Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
                rb.AddForce(bounceDirection * 500f); // Adjust force as needed
            }
        }
    }

    // Coroutine for damage cooldown
    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canDamagePlayer = true;
    }

    public void TakeDamage(float damage)
    {
        if (!canTakeDamage)
        {
            return;
        }
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            // Add effects when it dies
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}