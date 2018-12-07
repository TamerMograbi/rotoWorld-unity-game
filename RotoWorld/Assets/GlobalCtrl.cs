using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCtrl : MonoBehaviour {

    public static GlobalCtrl instance;
    public bool[] levelsCompleted;
    public int numOfLevels;

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        numOfLevels = 3; // 3 levels for now ( other than the main one)
        levelsCompleted = new bool[numOfLevels];
        for (int i = 0; i < numOfLevels; i++)
        {
            levelsCompleted[i] = false;
        }
    }
}
