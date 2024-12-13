using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playLevel1()
    {
        SceneManager.LoadScene("House");
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene("Garden");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
