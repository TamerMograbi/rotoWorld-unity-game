using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchController : MonoBehaviour {

    public bool hit;
    private bool started = false;
    public GameObject switchPad;
    public GameObject triggerObject;
	// Use this for initialization
	void Start () {
        hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (hit)
        {
            Destroy(switchPad);
            if (triggerObject.CompareTag("movingPlatform"))
            {
                triggerObject.GetComponent<MovingPlatformController>().started = true;
                if (!started)
                {
                    triggerObject.GetComponent<MovingPlatformController>().StartCoroutine(triggerObject.GetComponent<MovingPlatformController>().Countdown());
                    started = true;
                }
            }
        }
	}

    public void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player")) //default
        {
            hit = true;
            //Destroy(collision.gameObject);
        }
    }
}
