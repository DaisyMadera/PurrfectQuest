using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AudioClip[] swooshSoundClips;
    private Animator anim;
    public float attackRange = 1.0f;      // Range of the scratch attack
    public float attackDamage = 20f;      // Damage dealt by the scratch
    public float pushForce = 200f;        // Force applied to push the enemy
    public LayerMask enemyLayers;         // Layers that represent enemies

    public Transform attackPoint;         // Point from where the attack originates

    private bool canAttack = true;
    public float attackCooldown = 0.4f;

    private Vector2 lastMovementDirection;


    void Start()
    {
        anim = GetComponent<Animator>(); // Initialize the Animator
        if (anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canAttack)
        {
            Scratch();
        }
    }

    public void SetLastMovementDirection(Vector2 direction)
    {
        if (direction.magnitude > 0.1f)
            lastMovementDirection = direction.normalized;
    }

    void Scratch()
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(swooshSoundClips, transform, 1f);
        canAttack = false;
        StartCoroutine(AttackCooldown());

        // Set last movement direction based on input
        Vector2 movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        SetLastMovementDirection(movementDirection);

        anim.SetBool("isScratching", true);

        // Detect enemies in range of the attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage each enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            // Check if the enemy has an IDamageable component
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Apply damage to the enemy
                damageable.TakeDamage(attackDamage);

                // Apply force to push the enemy
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 pushDirection = enemy.transform.position - transform.position;
                    pushDirection.Normalize();
                    enemyRb.AddForce(pushDirection * pushForce);
                }
            }
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        anim.SetBool("isScratching", false);
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Draw the attack range in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
