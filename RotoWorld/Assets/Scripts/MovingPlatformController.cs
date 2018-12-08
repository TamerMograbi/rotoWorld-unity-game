using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour {

    public float xspeed;
    public float yspeed;
    public float zspeed;
    public bool started;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (started)
        transform.Translate(new Vector3(xspeed, yspeed, zspeed) * Time.deltaTime);
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            xspeed *= -1;
            yspeed *= -1;
            zspeed *= -1;
        }
    }
}
