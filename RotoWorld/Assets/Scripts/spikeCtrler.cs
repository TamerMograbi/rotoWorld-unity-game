using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeCtrl2 : MonoBehaviour {

    // Use this for initialization
    float offset;//how much spikes will go up
    int delay = 1; //number of seconds that spikes will stay up
    float yPos;
    void Start()
    {
        offset = transform.position.y + 3;
        delay = 1;
        yPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        yPos = Mathf.Lerp(yPos, offset, 0.1f);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
