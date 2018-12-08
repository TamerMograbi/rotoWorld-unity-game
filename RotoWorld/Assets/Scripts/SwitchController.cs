using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour {

    public bool hit;
    public GameObject switchLight;
    public GameObject triggerObject;
	// Use this for initialization
	void Start () {
        hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (hit)
        {
            switchLight.GetComponent<Renderer>().material = Resources.Load<Material>("Switch_Green");
            if (triggerObject.tag == "Moving Platform")
                triggerObject.GetComponent<MovingPlatformController>().started = true;
        }
	}

    public void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.tag);
        if (collision.gameObject.tag == "Rock") //default
        {
            hit = true;
            //Destroy(collision.gameObject);
        }
    }
}
