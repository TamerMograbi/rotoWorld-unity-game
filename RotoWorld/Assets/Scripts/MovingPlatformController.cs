using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour {

    public float xspeed;
    public float yspeed;
    public float zspeed;
    public bool started;
    public float reverseTime;
    public float currTime;

	// Use this for initialization
	void Start () {
        currTime = reverseTime;
        if (started)
        {
            StartCoroutine(Countdown());
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (started)
            transform.Translate(new Vector3(xspeed, yspeed, zspeed) * Time.deltaTime);

	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<AnimController>().IsGrounded())
                other.gameObject.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = null;
        }
    }

    public IEnumerator Countdown()
    {
        while (currTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            currTime -= 0.1f;
        }
        xspeed *= -1;
        yspeed *= -1;
        zspeed *= -1;
        currTime = reverseTime;
        StartCoroutine(Countdown());
    }
}
