using UnityEngine;

public class Bee : MonoBehaviour, IDamageable
{
    [Header("Bee Settings")]
    public float health = 1f;
    public float patrolSpeed = 2f;        // Speed during patrolling
    public float chaseSpeed = 4f;        // Speed when chasing the player
    public float detectionRange = 5f;    // Range within which the bee detects the player
    public float attackRange = 0.3f;       // Range within which the bee attacks the player
    public float attackCooldown = 1f;    // Time between attacks
    public int damage = 1;               // Damage dealt to the player on collision

    [Header("References")]
    public Transform player;             // Reference to the player
    public Animator animator;            // Animator for controlling animations
    public Transform[] waypoints;        // Waypoints for rectangular patrol
    public Collider2D roomBounds;        // Collider defining the room boundary

    private Rigidbody2D rb;              // Rigidbody for movement
    private int currentWaypointIndex = 0; // Current waypoint index
    private bool isChasing = false;      // Whether the bee is chasing the player
    private bool isAttacking = false;    // Whether the bee is currently attacking
    private float attackTimer = 0f;      // Timer for attack cooldown

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator.SetBool("IsAttacking", false); // Start in idle state
    }

    void Update()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0) isAttacking = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isChasing)
        {
            if (distanceToPlayer > detectionRange)
            {
                StopChasing();
            }
            else if (distanceToPlayer <= attackRange)
            {
                PerformAttack();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            if (distanceToPlayer <= detectionRange)
            {
                StartChasing();
            }
            else
            {
                Patrol();
            }
        }

        UpdateAnimatorParameters();
        StayInRoomBounds();
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        rb.velocity = direction * patrolSpeed;

        // Check if the bee has reached the current waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Move to the next waypoint
        }
    }

    private void StartChasing()
    {
        isChasing = true;
        rb.velocity = Vector2.zero; // Stop patrolling
        animator.SetBool("IsAttacking", false);
    }

    private void StopChasing()
    {
        isChasing = false;
        rb.velocity = Vector2.zero; // Stop chasing
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
    }

    private void PerformAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop moving during attack

        if (animator != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
            animator.SetBool("IsAttacking", true); // Trigger StingTree blend tree
        }

        attackTimer = attackCooldown;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        isAttacking = false;

        if (animator != null)
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    private void UpdateAnimatorParameters()
    {
        Vector2 velocity = rb.velocity.normalized;
        animator.SetFloat("X", velocity.x);
        animator.SetFloat("Y", velocity.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void StayInRoomBounds()
    {
        Vector2 position = transform.position;
        Vector2 roomMin = roomBounds.bounds.min;
        Vector2 roomMax = roomBounds.bounds.max;

        position.x = Mathf.Clamp(position.x, roomMin.x, roomMax.x);
        position.y = Mathf.Clamp(position.y, roomMin.y, roomMax.y);

        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            CharacterHealth playerHealth = collision.gameObject.GetComponent<CharacterHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        // Reduce the bee's health
        health -= damage;

        // Check if the bee's health is zero or below
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Stop all movement
        rb.velocity = Vector2.zero;

        // Destroy the bee after the death animation
        Destroy(gameObject, 1f); // Adjust delay to match the death animation duration
    }   
}