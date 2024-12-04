using UnityEngine;

// rn this fish give you an extra heart but i am going to edit it to let you be immune to everything
public class Fish : MonoBehaviour
{
    [SerializeField] private float healAmount = 1; // Amount of health the fish restores

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger detected with: {collision.gameObject.name}");

        // Try to get the CharacterHealth component
        CharacterHealth playerHealth = collision.GetComponent<CharacterHealth>();

        if (playerHealth != null && collision.CompareTag("Player"))
        {
            Debug.Log("Player detected. Attempting to heal...");

            // Heal the player if they are not at full health
            if (playerHealth.currentHealth < playerHealth.StartingHealth)
            {
                Debug.Log("Healing the player...");
                playerHealth.Heal(healAmount);

                // Destroy the fish after healing
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Player is already at full health.");
            }
        }
    }
}