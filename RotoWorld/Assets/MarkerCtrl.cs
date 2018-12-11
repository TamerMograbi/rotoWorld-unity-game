using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerCtrl : MonoBehaviour {

    bool letterIsInPlace;
    string letterCubeTag;
	// Use this for initialization
	void Start () {
        letterIsInPlace = false;
        letterCubeTag = GetLetterOfMarker();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(letterCubeTag))
        {
            letterIsInPlace = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag(letterCubeTag))
        {
            letterIsInPlace = false;
        }
    }

    public bool GetletterIsInPlace()
    {
        return letterIsInPlace;
    }

    //each marker belongs to one letter and according to the name of the marker, we find out which letter it belongs to
    private string GetLetterOfMarker()
    {
        string res ="";
        switch (name)
        {
            case "letter 1 marker":
                {
                    res = "letter1";
                    break;
                }
            case "letter 2 marker":
                {
                    res = "letter2";
                    break;
                }
            case "letter 3 marker":
                {
                    res = "letter3";
                    break;
                }
        }
        return res;
    }
}
