using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Collision happened");
        Debug.Log($"Collision detected with {collision.gameObject.name}");
    }

}

// [SerializeField] private float healAmount = 1; // Amount of health the fish restores

    // void Start(){

    // }
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     print("Collision happened");
    //     print($"Collided with: {collision.gameObject.name}");

    //     // Access the game object that collided
    //     CharacterHealth playerHealth = collision.transform.root.GetComponent<CharacterHealth>();
    //     print(playerHealth);
    //     if (playerHealth != null)
    //     {
    //         print("Player health is not null");
    //         print(collision.gameObject.root.CompareTag("Player"));
    //         print((playerHealth.currentHealth < playerHealth.StartingHealth));
    //         // Check if the player's health is less than full
    //         if (collision.gameObject.root.CompareTag("Player") && (playerHealth.currentHealth < playerHealth.StartingHealth))
    //         {
    //             print("Healing the player...");
    //             // Heal the player
    //             playerHealth.Heal(healAmount);

    //             // Optionally, destroy the fish after healing
    //             Destroy(gameObject);
    //         }
    //     }
    // }