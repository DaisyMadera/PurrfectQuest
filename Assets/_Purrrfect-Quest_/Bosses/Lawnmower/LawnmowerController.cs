using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LawnmowerController : MonoBehaviour, IDamageable
{
    public float detectionRange = 5f;
    public float chaseSpeed = 3f;
    public float wanderSpeed = 1.5f;
    public float maxHealth = 8f;
    public float damageToPlayer = 1f;
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

    // For dash attack
    public float dashForce = 1000f;
    private Vector2 lastPlayerPosition;

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
            lastPlayerPosition = playerTransform.position;
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

        // Show health bar initially (you can hide if you want)
        healthBarCanvas.gameObject.SetActive(true);

        // Start wandering
        InvokeRepeating("SetRandomDirection", 0f, 1f); // Change direction every 2 seconds

        // Start the attack routine
        StartCoroutine(AttackRoutine());
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        // Track last player position
        lastPlayerPosition = playerTransform.position;

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
    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (playerTransform != null)
            {
                DashAttack(lastPlayerPosition);
            }
        }
    }

    void DashAttack(Vector2 targetPosition)
    {
        // Calculate direction towards the last known player position
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        // Add force for a dash
        rb.AddForce(direction * dashForce);
    }
}
