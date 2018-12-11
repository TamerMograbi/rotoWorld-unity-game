using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessSwitchController : MonoBehaviour {

    public bool hit;
    private bool started = false;
    public GameObject switchPad;
    public Text guessText;
    private float waitTime = 2.0f;
	// Use this for initialization
	void Start () {
        hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*if (hit)
        { 
            if (triggerObject.CompareTag("movingPlatform"))
            {
                triggerObject.GetComponent<MovingPlatformController>().started = true;
                if (!started)
                {
                    triggerObject.GetComponent<MovingPlatformController>().StartCoroutine(triggerObject.GetComponent<MovingPlatformController>().Countdown());
                    started = true;
                }
            }
        }*/
	}

    public void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player") && !hit) //default
        {
            hit = true;
            FindObjectOfType<AudioManager>().Play("switch");
            switchPad.transform.position = new Vector3(switchPad.transform.position.x, switchPad.transform.position.y - .035f, switchPad.transform.position.z);
            StartCoroutine(Countdown());
            if(FindObjectOfType<GameMasterCtrl>().GameWon())
            {
                StartCoroutine(GuessTextTimer("VICTORY"));
            }
            else
            {
                StartCoroutine(GuessTextTimer("WRONG LETTER COMBO!"));
            }
            
        }
    }

    public IEnumerator Countdown()
    {
        while (waitTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            waitTime -= 0.1f;
        }
        waitTime = 2.0f;
        hit = false;
        FindObjectOfType<AudioManager>().Play("switch");
        switchPad.transform.position = new Vector3(switchPad.transform.position.x, switchPad.transform.position.y + .035f, switchPad.transform.position.z);
    }

    public IEnumerator GuessTextTimer(string str)
    {
        guessText.text = str;
        yield return new WaitForSeconds(2f);
        guessText.text = "";
    }
}
