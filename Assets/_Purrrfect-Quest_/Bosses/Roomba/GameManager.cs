using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int totalBases;
    private int basesDestroyed = 0;

    void Start()
    {
        // Count total bases at the start
        totalBases = GameObject.FindGameObjectsWithTag("Base").Length;
    }

    public void BaseDestroyed()
    {
        basesDestroyed++;
    }

    public bool AllBasesDestroyed()
    {
        return basesDestroyed >= totalBases;
    }
}