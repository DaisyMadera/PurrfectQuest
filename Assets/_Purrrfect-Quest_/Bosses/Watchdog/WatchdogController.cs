using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchdogController : MonoBehaviour, IDamageable
{
    [Header("Movement Settings")]
    public float detectionRange = 5f;
    public float chaseSpeed = 3f;
    public float wanderSpeed = 1.5f;

    [Header("Health Settings")]
    public float maxHealth = 8f;
    private float currentHealth;

    [Header("Attack Settings")]
    public float damageToPlayer = 1f;
    public float bounceForce = 500f;
    public float damageCooldown = 3f;  // Player can only be damaged by this boss every 3 seconds
    private float playerNextDamageTime = 0f; // Tracks when player can be damaged again

    [Header("Bark Stun Settings")]
    public float barkStunInterval = 5f;
    public float barkDuration = 3f;
    public float barkMaxRadius = 3f;
    public float stunDuration = 2f;
    public int barkRepeatCount = 3;

    // Private variables
    private Transform playerTransform;
    private Rigidbody2D rb;
    private Animator anim;
    private GameManager gameManager;
    private PlayerMovement playerMovement;
    private CharacterHealth playerHealth;

    // State variables
    private Vector2 movement;
    private bool playerDetected = false;
    private bool isBarkingOrStunning = false;
    private bool canTakeDamage = true;

    // UI
    public Canvas healthBarCanvas;
    public Image healthBarImage;

    // Bark stun visual effect
    public GameObject barkRadiusVisualPrefab;
    private GameObject currentBarkRadiusVisual;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Find player
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            playerMovement = playerObject.GetComponent<PlayerMovement>();
            playerHealth = playerObject.GetComponent<CharacterHealth>();
        }
        else
        {
            Debug.LogError("Player object not found. Ensure Player has 'Player' tag.");
        }

        // Setup health bar
        gameManager = GameObject.FindObjectOfType<GameManager>();
        healthBarCanvas.gameObject.SetActive(true);

        // Start wandering
        InvokeRepeating("SetRandomDirection", 0f, 1f);
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Player detection logic
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool currentlyDetecting = (distanceToPlayer <= detectionRange);

        HandlePlayerDetection(currentlyDetecting);
        UpdateMovement();
        UpdateAnimator();
    }

    void HandlePlayerDetection(bool currentlyDetecting)
    {
        // Player detection state changes
        if (currentlyDetecting && !playerDetected)
        {
            playerDetected = true;
            StopAllCoroutines();
            StartCoroutine(BarkStunRoutine());
        }
        else if (!currentlyDetecting && playerDetected)
        {
            playerDetected = false;
            StopAllCoroutines();
            InvokeRepeating("SetRandomDirection", 0f, 1f);
        }
    }

    void UpdateMovement()
    {
        // Movement logic
        if (playerDetected && !isBarkingOrStunning )
        {
            // Chase the player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            movement = direction * chaseSpeed;
        }
        else if (!playerDetected && !isBarkingOrStunning )
        {
            // Wander randomly
            movement = movement.normalized * wanderSpeed;
        }
        else
        {
            // During bark/stun or dash, no normal movement
            movement = Vector2.zero;
        }
    }

    void UpdateAnimator()
    {
        // Update Animator parameters
        anim.SetFloat("X", movement.x);
        anim.SetFloat("Y", movement.y);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }

    // Bark/Stun Routine
    private IEnumerator BarkStunRoutine()
    {
        while (playerDetected)
        {
            yield return new WaitForSeconds(barkStunInterval);
            if (playerDetected)
            {
                yield return PerformBarkStunAttack();
            }
        }
    }

    private IEnumerator PerformBarkStunAttack()
    {
        isBarkingOrStunning = true;
        movement = Vector2.zero;

        // Repeat barking multiple times
        for (int i = 0; i < barkRepeatCount; i++)
        {
            anim.SetTrigger("Bark");

            // Create bark radius visual
            CreateBarkRadiusVisual();

            // Wait during bark
            yield return new WaitForSeconds(barkDuration / barkRepeatCount);

            // Stun player if within radius
            if (Vector2.Distance(transform.position, playerTransform.position) <= barkMaxRadius)
            {
                StunPlayer();
            }

            // Remove bark radius visual
            if (currentBarkRadiusVisual != null)
            {
                Destroy(currentBarkRadiusVisual);
            }
        }

        isBarkingOrStunning = false;
    }

    void CreateBarkRadiusVisual()
    {
        // Instantiate a circle sprite at the dog's position
        if (barkRadiusVisualPrefab != null)
        {
            currentBarkRadiusVisual = Instantiate(barkRadiusVisualPrefab, transform.position, Quaternion.identity);
            // Animate the scale to match bark radius
            StartCoroutine(AnimateBarkRadiusVisual());
        }
    }

    IEnumerator AnimateBarkRadiusVisual()
    {
        float startTime = Time.time;
        float animationDuration = barkDuration / barkRepeatCount;
        while (Time.time < startTime + animationDuration)
        {
            float t = (Time.time - startTime) / animationDuration;
            float currentScale = Mathf.Lerp(0f, barkMaxRadius * 2, t);

            if (currentBarkRadiusVisual != null)
            {
                currentBarkRadiusVisual.transform.localScale = new Vector3(currentScale, currentScale, 1);
            }

            yield return null;
        }
    }

    void StunPlayer()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            Invoke("UnstunPlayer", stunDuration);
        }
    }

    void UnstunPlayer()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Check if player can be damaged now
            if (Time.time >= playerNextDamageTime)
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageToPlayer);

                    // Bounce off the player
                    Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
                    rb.AddForce(bounceDirection * bounceForce);

                    // Set the time when player can be damaged next
                    playerNextDamageTime = Time.time + damageCooldown;
                }
            }
        }
    }

    // Damage and health methods
    public void TakeDamage(float damage)
    {
        if (!canTakeDamage) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }

    void SetRandomDirection()
    {
        if (playerTransform != null &&
            Vector2.Distance(transform.position, playerTransform.position) > detectionRange &&
            !playerDetected)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            movement = direction * wanderSpeed;
        }
    }
}
