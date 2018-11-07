using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    Vector3 offset;
    AnimController animCtrl;
    public AnimController.gravityDirection gravityDir, prevGravityDir;
    Vector3 yOffset;
    Vector3 normalGravityOffset = new Vector3(0, 2, 0);
    Vector3 InvertedGravityOffset = new Vector3(0, -2, 0);

    public float angle;
    public bool rotated;
    // Use this for initialization
    void Start () {
        offset = transform.position - player.transform.position;
        animCtrl = player.GetComponent<AnimController>();
        rotated = false;
        angle = 30;
        prevGravityDir = AnimController.gravityDirection.DOWN;

    }
	
	// Update is called once per frame
	void Update () {
        if(animCtrl.getGravityDir() != gravityDir)
        {
            prevGravityDir = gravityDir;
            gravityDir = animCtrl.getGravityDir();
        }
        if (gravityDir == AnimController.gravityDirection.UP)
        {
            yOffset = InvertedGravityOffset;
            if (!rotated)
            {
                angle = Mathf.LerpAngle(angle, -30, Time.deltaTime);
                transform.eulerAngles = new Vector3(angle, 0, 0);
                if (Mathf.Abs(-30 - transform.rotation.eulerAngles.x ) < 1) // if reached desired angle the stop rotating
                {
                    rotated = true;
                }
            }

        }
        else if(gravityDir == AnimController.gravityDirection.DOWN)
        {
            yOffset = normalGravityOffset;
        }
        //transform.position = player.transform.position + offset + yOffset;
        Vector3 targetPos = player.transform.position + offset + yOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
    }

}
