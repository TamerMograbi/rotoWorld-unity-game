using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigCtrl : MonoBehaviour {

    // Use this for initialization
    spikeCtrll spikeCtrl;
	void Start () {
        spikeCtrl = GetComponentInParent<spikeCtrll>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spikeCtrl.StartMovingSpikes();
        }
    }
}
