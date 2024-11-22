using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth{get; private set;}
    private Animator anim;
    private bool dead;

    public GameObject gameOverScreen;
    public Button restartButton;

    private void Start(){
        gameOverScreen.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    } 
    private void Awake ()
    {
        currentHealth = startingHealth;
        anim = GetComponent < Animator>();
    }
    public void TakeDamage(float damage)
    {
        currentHealth= Mathf.Clamp( currentHealth - damage,0, startingHealth);
        
         

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");//player hurt 

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

            if (stateInfo.IsName ("Die") && normalizedTime  >= 1f)
            {
                ShowGameOver();
                break;
            }
            yield return null;
        }

    }
    private void ShowGameOver(){
        gameOverScreen.SetActive(true);
    }
    
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
