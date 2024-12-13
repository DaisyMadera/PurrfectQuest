using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Initializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player active: " + (FindObjectOfType<PlayerMovement>() != null));
        Debug.Log("Time scale: " + Time.timeScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
