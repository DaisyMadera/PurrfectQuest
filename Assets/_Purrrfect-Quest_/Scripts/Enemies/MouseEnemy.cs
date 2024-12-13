using System.Collections;
using UnityEngine;

public class MouseEnemy : MonoBehaviour, IDamageable
{
    public Transform startPosition; // Position where the mouse starts (hole)
    public float detectionRadius = 8.0f;
    public float speed = 5.0f; // Base speed
    public float retreatSpeedMultiplier = 1.5f; // Multiplier for retreating speed
    public float health = 50f; // Health value of the mouse
    public GameObject dropItemPrefab; // Item to drop when hit
    public float damageToPlayer = 1.0f; // Amount of health to reduce on collision
    public float knockbackForce = 500f; // Force applied when knocked back

    private enum State
    {
        Idle,
        ChasingPlayer,
        RunningBackAfterStealing
    }

    private State currentState = State.Idle;
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private bool hasStolenHeart = false; // Ensures one heart stolen per interaction
    private Vector2 movement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        transform.position = startPosition.position; // Set initial position to start
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            Debug.LogError("Player object not found in the scene. Please ensure the Player object has the tag 'Player'.");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                DetectPlayer();
                break;

            case State.ChasingPlayer:
                ChasePlayer();
                break;

            case State.RunningBackAfterStealing:
                RunBackToStart();
                break;
        }

        UpdateAnimatorParameters();
    }

    void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            currentState = State.ChasingPlayer;
            Debug.Log("Mouse detected the player!");
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        movement = (player.position - transform.position).normalized;
        rb.velocity = movement * speed;
    }

    void RunBackToStart()
    {
        movement = (startPosition.position - transform.position).normalized;
        rb.velocity = movement * speed * retreatSpeedMultiplier; // Runs back casually

        if (Vector3.Distance(transform.position, startPosition.position) < 0.1f)
        {
            ResetMouse();
        }
    }

    void ResetMouse()
    {
        currentState = State.Idle;
        hasStolenHeart = false; // Reset stolen heart flag
        rb.velocity = Vector2.zero;
        transform.position = startPosition.position; // Reset position to hole
        Debug.Log("Mouse returned to its hole and reset.");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            DropCheese();
            ApplyKnockback();
            currentState = State.RunningBackAfterStealing; // Switch to retreat after damage
            Debug.Log("Mouse took damage but is still alive!");
        }
    }

    void ApplyKnockback()
    {
        if (player == null) return;

        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    void DropCheese()
    {
        if (dropItemPrefab == null) return;

        Vector3 dropPosition = transform.position + (Vector3)(Random.insideUnitCircle * 0.5f); // Slightly offset
        Instantiate(dropItemPrefab, dropPosition, Quaternion.identity);
        Debug.Log("Mouse dropped a piece of cheese!");
    }

    void Die()
    {
        Debug.Log("Mouse has been defeated!");
        DropCheese(); // Drop cheese on death
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !hasStolenHeart)
        {
            // Steal a heart
            var playerHealth = collision.collider.GetComponent<CharacterHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
                Debug.Log("Mouse stole a heart!");
            }

            hasStolenHeart = true;
            currentState = State.RunningBackAfterStealing; // Transition to running back after stealing
        }
    }

    private void UpdateAnimatorParameters()
    {
        // Update X and Y parameters based on the current movement direction
        if (movement != Vector2.zero)
        {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
        }
    }
}
