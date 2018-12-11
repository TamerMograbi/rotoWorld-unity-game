using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCubeCtrl : MonoBehaviour {

    // Use this for initialization
    int levelIdx = 0;
    string objName;
    Renderer[] renderers;


    void Start () {
        objName = this.gameObject.name;
        if (objName == "level 1 letter cube")
        {
            levelIdx = 0;
        }
        else if(objName == "level 2 letter cube")
        {
            levelIdx = 1;
        }
        else if(objName == "level 3 letter cube")
        {
            levelIdx = 2;//training level index
        }
        this.GetComponent<Renderer>().enabled = false;
        renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            r.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
        if(!this.GetComponent<Renderer>().enabled && GlobalCtrl.instance.levelsCompleted[levelIdx])
        {
            this.GetComponent<Renderer>().enabled = true;
            foreach(var r in renderers)
            {
                r.enabled = true;
            }
        }

	}
}
