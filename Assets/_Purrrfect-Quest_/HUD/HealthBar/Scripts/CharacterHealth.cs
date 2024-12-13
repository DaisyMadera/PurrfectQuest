using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private AudioClip hurtSoundClip;
    public float currentHealth; // Now public and visible in Inspector
    private Animator anim;
    private bool dead;
    private GameOverScreen gameOverScreen;
    public float StartingHealth => startingHealth;


     
    private void Awake ()
    {
        currentHealth = startingHealth;
        anim = GetComponent < Animator>();
        
        // Find the GameOverScreen in the scene
        gameOverScreen = FindObjectOfType<GameOverScreen>();
    }
    public void TakeDamage(float damage)
    {
        currentHealth= Mathf.Clamp( currentHealth - damage,0, startingHealth);
        
         
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");//player hurt 
            SoundFXManager.instance.PlaySoundFXClip(hurtSoundClip, transform, 1f);

        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");//player dead
                GetComponent< PlayerMovement>().enabled = false;
                dead = true;
                StartCoroutine(AfterDeathAnim());
            }
            
        }
    }
    private IEnumerator AfterDeathAnim(){
        while (true)
        {
            AnimatorStateInfo stateInfo= anim.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = Mathf.Clamp(stateInfo.normalizedTime, 0f, 1f);

            if (gameOverScreen != null)
            {
                gameOverScreen.ShowGameOver(); // Call the ShowGameOver method
            }
            yield return null;
        }

    }
    
    
    

    public void Heal(float amount)
    {
        if (!dead) // Ensure the player isn't dead
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
            Debug.Log($"Player healed. Current health: {currentHealth}/{startingHealth}");
        }
    }
}


