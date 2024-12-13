using UnityEngine;

public class Skunk : MonoBehaviour
{
    [Header("Movement Settings")]
    public float runSpeed = 10f;          // Speed of the skunk when running
    public float gasRange = 2f;          // Range within which the skunk can attack
    public int maxAttacks = 5;           // Number of attacks before returning to the bush
    public float returnSpeed = 5f;       // Speed of the skunk when returning to the bush

    [Header("References")]
    public Transform player;             // Reference to the player
    public Transform bushPosition;       // Position of the bush to return to
    public Animator animator;            // Animator for controlling animations

    private Rigidbody2D rb;              // Rigidbody for skunk movement
    private bool isRunning = false;      // Whether the skunk is currently running
    private bool isAttacking = false;    // Whether the skunk is currently attacking
    private bool isReturning = false;    // Whether the skunk is returning to the bush
    private int attackCount = 0;         // Tracks the number of attacks performed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator.SetBool("IsRunning", false); // Start in idle state
    }

    void Update()
    {
        if (isReturning)
        {
            ReturnToBush();
            return;
        }

        if (!isRunning || isAttacking) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Move to the player if not in attack range
        if (distanceToPlayer > gasRange)
        {
            MoveToPlayer();
        }
        else if (!isAttacking && attackCount < maxAttacks)
        {
            AttackPlayer();
        }
        else if (attackCount >= maxAttacks)
        {
            isReturning = true; // Skunk returns to the bush after attacking
        }
    }

    private void MoveToPlayer()
    {
        if (!isRunning) return;

        // Move directly to the player's position
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * runSpeed;

        animator.SetBool("IsRunning", true); // Trigger running animation

        // Stop running if close enough to the player
        if (Vector2.Distance(transform.position, player.position) <= gasRange)
        {
            rb.velocity = Vector2.zero;
            isRunning = false;
            animator.SetBool("IsRunning", false); // Transition back to idle
            Debug.Log("Skunk reached player.");
        }
    }

    private void AttackPlayer()
    {
        if (isAttacking) return;

        isAttacking = true;
        rb.velocity = Vector2.zero; // Stop movement

        if (animator != null)
        {
            animator.SetTrigger("Attack"); // Trigger attack animation
            Debug.Log("Skunk triggered attack animation.");
        }

        attackCount++; // Increment attack counter
        Debug.Log($"Skunk attacked {attackCount}/{maxAttacks} times.");

        // Reset attacking state after the attack animation
        Invoke(nameof(ResetAttack), 1f); // Adjust time to match the attack animation duration
    }

    private void ResetAttack()
    {
        Debug.Log("Skunk finished attack motion.");
        isAttacking = false;

        // If attack limit is reached, start returning to the bush
        if (attackCount >= maxAttacks)
        {
            isReturning = true;
            animator.SetBool("IsRunning", true); // Trigger running animation for return
        }
        else
        {
            // Re-enable running if the player moves out of gas range
            if (Vector2.Distance(transform.position, player.position) > gasRange)
            {
                isRunning = true;
                animator.SetBool("IsRunning", true);
            }
        }
    }

    private void ReturnToBush()
    {
        if (bushPosition == null)
        {
            Debug.LogError("Bush position is not assigned!");
            return;
        }

        // Move directly to the bush position
        Vector2 direction = (bushPosition.position - transform.position).normalized;
        rb.velocity = direction * returnSpeed;

        Debug.Log($"Skunk returning to bush. Position: {transform.position}, Target: {bushPosition.position}");

        // Stop when reaching the bush
        if (Vector2.Distance(transform.position, bushPosition.position) < 0.1f)
        {
            rb.velocity = Vector2.zero;
            isReturning = false;

            // Trigger idle animation and reset the skunk
            animator.SetBool("IsRunning", false);
            Debug.Log("Skunk returned to bush and reset.");
            ResetSkunk();
        }
    }

    private void ResetSkunk()
    {
        isRunning = false;
        isAttacking = false;
        attackCount = 0;
        // Skunk remains idle until triggered again
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            isRunning = true;

            // Trigger the running animation
            if (animator != null)
            {
                animator.SetBool("IsRunning", true);
            }
        }
    }

    public void ApplyGasDamage()
    {
        // Called via Animation Event to synchronize gas attack damage
        if (Vector2.Distance(transform.position, player.position) < gasRange)
        {
            Debug.Log("Gas damage applied to player.");
            CharacterHealth playerHealth = player.GetComponent<CharacterHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Player loses a heart
            }
        }
        else
        {
            Debug.Log("Player is out of gas range, no damage applied.");
        }
    }




    //public void TriggerDeath()
    //{
     //   isRunning = false;
     //   rb.velocity = Vector2.zero; // Stop movement

    //    if (animator != null)
     //   {
    //        animator.SetTrigger("Dies"); // Trigger death animation
     //       Debug.Log("Skunk triggered death animation.");
    //    }

        // Destroy skunk after death animation completes
    //    Destroy(gameObject, 2f); // Adjust delay to match death animation length
    //}
}