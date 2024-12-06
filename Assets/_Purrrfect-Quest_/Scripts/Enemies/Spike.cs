using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float damage;
    public float maxDistance = 5f;
    private Vector3 startPosition;
    void Start()
    {
        // Record the initial position when the spike is spawned
        startPosition = transform.position;
    }

    void Update()
    {
        // Check the distance the spike has traveled from the start position
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        // If the spike has traveled farther than the max distance, destroy it
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);  // Destroy the spike
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collisions with the player and other objects
        // Check if the spike collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterHealth playerHealth = collision.gameObject.GetComponent<CharacterHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);  // Apply damage to the player
            }
            Destroy(gameObject);  // Destroy the spike after it hits the player
        }
    }

    
}
