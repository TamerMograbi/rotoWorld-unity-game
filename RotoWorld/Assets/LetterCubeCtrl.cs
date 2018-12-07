using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCubeCtrl : MonoBehaviour {

    // Use this for initialization
    int levelIdx = 0;
    string name;

    void Start () {
        this.GetComponent<Renderer>().enabled = false;
        name = this.gameObject.name;
        if (name == "level 1 letter cube")
        {
            levelIdx = 0;
        }
        else if(name == "level 2 letter cube")
        {
            levelIdx = 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
        if(!this.GetComponent<Renderer>().enabled && GlobalCtrl.instance.levelsCompleted[levelIdx])
        {
            this.GetComponent<Renderer>().enabled = true;
        }

	}
}
