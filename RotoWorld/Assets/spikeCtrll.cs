using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeCtrll : MonoBehaviour {

    // Use this for initialization
    float offset;//how much spikes will go up
    float delay = 2f; //number of seconds that spikes will stay up
    float yPos;
    float originalY;
    bool reachedUp;
    bool movingUpAndDown;
    GameObject player;
    AnimController playerCtrl;
    void Start()
    {
        offset = transform.position.y + 0.9f; //might need to change according to gravity
        delay = 1;
        yPos = transform.position.y;
        originalY = yPos;
        reachedUp = false;
        movingUpAndDown = false;
        player = GameObject.Find("Player");
        playerCtrl = player.GetComponent<AnimController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movingUpAndDown)
        {
            moveSpikes();
        }
        
    }

    public void moveSpikes()
    {
        if (Mathf.Abs(offset - yPos) > 0.1 && !reachedUp)
        {
            //yPos = Mathf.Lerp(yPos, offset, 0.1f);
            StartCoroutine(MoveUp());
        }
        else
        {
            reachedUp = true;
            StartCoroutine(backToOrginalPos());
        }
        //which axis is changed also depends on gravity
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    public IEnumerator backToOrginalPos()
    {
        yield return new WaitForSeconds(delay);
        yPos = Mathf.Lerp(yPos, originalY, 0.1f);
        if(Mathf.Abs(yPos - originalY) < 0.05)
        {
            movingUpAndDown = false; // moving spikes ended, with them back at original place
            reachedUp = false;
            yPos = originalY;
        }
    }

    public IEnumerator MoveUp()
    {
        yield return new WaitForSeconds(delay);
        yPos = Mathf.Lerp(yPos, offset, 0.1f);
    }

    public void StartMovingSpikes()
    {
        movingUpAndDown = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerCtrl.takeDamage();
        }
    }


}


