using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterCtrl : MonoBehaviour {

    MarkerCtrl marker1Ctrl;
    MarkerCtrl marker2Ctrl;
    MarkerCtrl marker3Ctrl;
    // Use this for initialization
    void Start () {
        marker1Ctrl = GameObject.Find("letter 1 marker").GetComponent<MarkerCtrl>();
        marker2Ctrl = GameObject.Find("letter 2 marker").GetComponent<MarkerCtrl>();
        marker3Ctrl = GameObject.Find("letter 3 marker").GetComponent<MarkerCtrl>();
    }
	
	// Update is called once per frame
	void Update () {
        if (marker1Ctrl.GetletterIsInPlace())
        {
            Debug.Log("letter L is in place");
        }
        if (marker2Ctrl.GetletterIsInPlace())
        {
            Debug.Log("letter 2 is in place");
        }
        if(marker3Ctrl.GetletterIsInPlace())
        {
            Debug.Log("letter 3 is in place");
        }
    }
}
