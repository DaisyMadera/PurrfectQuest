using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector2 movement;
    private Vector2 lastMovementDirection;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;    // The speed during the dash
    [SerializeField] private float dashDuration = 0.2f; // How long the dash lasts in seconds
    [SerializeField] private float dashCooldown = 1f;   // How long before you can dash again
    private float lastDashTime = -Mathf.Infinity;
    private bool isDashing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("IsWalking", true);

            // Update last movement direction
            lastMovementDirection = movement.normalized;
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void OnDash(InputValue value)
    {
        // Check if we can dash: cooldown passed and we have a direction
        if (!isDashing && Time.time - lastDashTime >= dashCooldown && lastMovementDirection.sqrMagnitude > 0)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        // Set the dash velocity
        rb.velocity = lastMovementDirection * dashSpeed;

        // Wait for the dash to end
        yield return new WaitForSeconds(dashDuration);

        // After dashing, return to normal movement
        isDashing = false;
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            // Normal movement
            rb.velocity = movement * speed;
        }
    }
}
