using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnemy : MonoBehaviour, IDamageable
{
    public Transform startPosition; // Position where the mouse starts
    public Transform endPosition; // Position where the mouse taunts and then returns to start
    public float speed = 2.0f;
    public float tauntDuration = 2.0f;
    public float detectionRadius = 5.0f;
    public float cooldownDuration = 5.0f; // Cooldown duration after returning to start
    public float health = 50f; // Health value of the mouse
    public GameObject dropItemPrefab;

    private enum State
    {
        Idle,
        RunningToEnd,
        Taunting,
        RunningBackToStart,
        Cooldown
    }

    private State currentState = State.Idle;
    private Transform player;
    private float tauntTimer = 0.0f;
    private float cooldownTimer = 0.0f;
    private Animator animator; 
    private bool isInterrupted = false;
    private PlayerMovement playerMovement; // Reference to the player's movement script

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = startPosition.position; // Set initial position to start
        animator = GetComponent<Animator>(); // Get animator component
        playerMovement = player.GetComponent<PlayerMovement>(); // Get the player's movement script

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on Player object.");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                DetectPlayer();
                animator.SetBool("isIdle", true);
                break;

            case State.RunningToEnd:
                if (playerMovement != null)
                {
                    playerMovement.enabled = false; // Prevent the player from moving
                }
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalkingDown", true);
                MoveToPosition(endPosition.position);
                if (Vector3.Distance(transform.position, endPosition.position) < 0.1f)
                {
                    currentState = State.Taunting;
                    tauntTimer = tauntDuration;
                    animator.SetBool("isWalkingDown", false);
                    animator.SetTrigger("taunt"); // Trigger taunt animation
                    Debug.Log("Mouse is taunting!");
                }
                break;

            case State.Taunting:
                tauntTimer -= Time.deltaTime;
                // animator.SetBool("isIdle", false);
                animator.SetBool("isTaunting", true);
                if (playerMovement != null)
                    {
                        playerMovement.enabled = true; // Allow the player to move again
                    }
                if (isInterrupted)
                {
                    isInterrupted = false;
                    currentState = State.RunningBackToStart;
                    animator.SetBool("isTaunting", false);
                    
                }
                else if (tauntTimer <= 0)
                {
                    currentState = State.RunningBackToStart;
                    animator.SetBool("isTaunting", false);
                }
                break;

            case State.RunningBackToStart:
                MoveToPosition(startPosition.position);
                animator.SetBool("isWalkingUp", true);
                if (Vector3.Distance(transform.position, startPosition.position) < 0.1f)
                {
                    currentState = State.Cooldown;
                    cooldownTimer = cooldownDuration;
                    animator.SetBool("isWalkingUp", false);
                    Debug.Log("Mouse is back at the start position. Entering cooldown.");
                }
                break;

            case State.Cooldown:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    currentState = State.Idle;
                    Debug.Log("Cooldown complete. Mouse can return.");
                    animator.SetBool("isIdle", true);
                }
                break;
        }
    }

    void DetectPlayer()
    {
        if (currentState == State.Idle && Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            // animator.SetBool("isIdle", false);
            animator.SetTrigger("alert"); // Trigger alert animation
            Debug.Log("Mouse detected the player!");
            Invoke(nameof(StartMovingToEnd), 2f);
        }
    }

    void StartMovingToEnd()
    {
        currentState = State.RunningToEnd;
        Debug.Log("Delay complete. Mouse is now moving to the end position.");
    }

    void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        if (currentState == State.Idle || currentState == State.Cooldown)
        {
            Debug.Log("Mouse cannot take damage while idle.");
            return;
        }
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("Mouse took damage while in state: " + currentState);
            if (currentState == State.Taunting)
            {
                isInterrupted = true;
                // Trigger injured animation
            }
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
