using System.Collections;
using UnityEngine;

public class MouseEnemy : MonoBehaviour, IDamageable
{
    public Transform startPosition; // Position where the mouse starts (hole)
    public float speed = 5.0f;
    public float detectionRadius = 8.0f;
    public float health = 50f; // Health value of the mouse
    public GameObject dropItemPrefab;

    private enum State
    {
        Idle,
        ChasingPlayer,
        Stealing,
        RunningBackToStart
    }

    private State currentState = State.Idle;
    private Transform player;
    private Animator animator; 
    private Vector3 chaseDirection;
    private bool hasStolenHeart = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        transform.position = startPosition.position; // Set initial position to start
        animator = GetComponent<Animator>();

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

            case State.RunningBackToStart:
                RunBackToStart();
                break;
        }
    }

    void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            currentState = State.ChasingPlayer;
            animator.SetTrigger("alert"); // Play alert animation
            Debug.Log("Mouse detected the player!");
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        chaseDirection = (player.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        animator.SetBool("isWalking", true);

        if (Vector3.Distance(transform.position, player.position) < 0.5f && !hasStolenHeart)
        {
            StealHeart();
        }
    }

    void StealHeart()
    {
        hasStolenHeart = true;
        currentState = State.RunningBackToStart;
        animator.SetTrigger("steal");
        Debug.Log("Mouse stole a heart!");

        // Decrease player's health
        var playerHealth = player.GetComponent<CharacterHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1); // Steal one heart
        }
    }

    void RunBackToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition.position, speed * Time.deltaTime);
        animator.SetBool("isWalking", true);

        if (Vector3.Distance(transform.position, startPosition.position) < 0.1f)
        {
            ResetMouse();
        }
    }

    void ResetMouse()
    {
        currentState = State.Idle;
        hasStolenHeart = false;
        animator.SetBool("isWalking", false);
        transform.position = startPosition.position;
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
            Debug.Log("Mouse took damage!");
        }
    }

    void Die()
    {
        Debug.Log("Mouse has been defeated!");
        // Play death animation 
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
