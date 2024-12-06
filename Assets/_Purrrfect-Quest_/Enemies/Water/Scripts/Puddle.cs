using UnityEngine;

public class Puddle : MonoBehaviour
{
    [SerializeField] private float damage; // Damage dealt by the puddle

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle player health
            CharacterHealth playerHealth = other.GetComponent<CharacterHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Deal damage to the player
            }
            Destroy(gameObject);
        }
    }
}
