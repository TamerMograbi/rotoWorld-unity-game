using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(name == "level 1 entrance")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
            }
            if(name == "level 2 entrance")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
            }
        }
    }
}
