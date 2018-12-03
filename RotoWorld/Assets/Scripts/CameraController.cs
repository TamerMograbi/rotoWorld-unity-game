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
    Vector3 leftGravityOffset = new Vector3(2, 0, 0);
    Vector3 rightGravityOffset = new Vector3(-2, 0, 0);

   // public float angle;
    public bool rotated;
    public bool rotating;

    private float xangle = 0.0f;    // (-30, 30) if gravity is on top or bottom, 0 if it is on sides
    private float yangle = 0.0f;    // (-30, 30) if gravity is on top or bottom, 0 if it is on sides
    private float zangle = 0.0f;    // same angle as player rotation (0, 90, 180, -90)
    //private float ztarget = 0.0f;
    // Use this for initialization


    void Start () {
        player = GameObject.Find("Player");
        offset = transform.position - player.transform.position;
        animCtrl = player.GetComponent<AnimController>();
        rotated = false;
        rotating = false;
        xangle = 30;
        yangle = 0;
        zangle = 0;
        prevGravityDir = AnimController.gravityDirection.DOWN;

    }
    // Update is called once per frame
    void LateUpdate () {
        if(animCtrl.getGravityDir() != gravityDir)
        {
            prevGravityDir = gravityDir;
            gravityDir = animCtrl.getGravityDir();
            rotated = false;
        }
        if (gravityDir == AnimController.gravityDirection.UP)
        {
            yOffset = InvertedGravityOffset;
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, -30, 3 * Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, 0, 3 * Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, 180, 3 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(-30 - xangle) < .1f && Mathf.Abs(0 - yangle) < .1f && Mathf.Abs(zangle - 180) < .1f)
                {
                    transform.eulerAngles = new Vector3(-30, 0, 180);
                    rotated = true;
                    rotating = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(xangle, yangle, zangle);
                }
            }

        }
        else if (gravityDir == AnimController.gravityDirection.LEFT)
        {
            yOffset = leftGravityOffset;
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, 0, 3 * Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, -30, 3 * Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, -90, 3 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(0 - xangle) < .1f && Mathf.Abs(-30 - yangle) < .1f && Mathf.Abs(zangle - -90) < .1f)
                {
                    transform.eulerAngles = new Vector3(0, -30, -90);
                    rotated = true;
                    rotating = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(xangle, yangle, zangle);
                }
            }

        }
        else if (gravityDir == AnimController.gravityDirection.DOWN)
        {
            yOffset = normalGravityOffset;
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, 30, 3 * Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, 0, 3 * Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, 0, 3 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(30 - xangle) < .1f && Mathf.Abs(0 - yangle) < .1f && Mathf.Abs(zangle - 0) < .1f)
                {
                    transform.eulerAngles = new Vector3(30, 0, 0);
                    rotated = true;
                    rotating = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(xangle, yangle, zangle);
                }
            }
        }
        else if (gravityDir == AnimController.gravityDirection.RIGHT)
        {
            yOffset = rightGravityOffset;
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, 0, 3 * Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, 30, 3 * Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, 90, 3 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(0 - xangle) < .1f && Mathf.Abs(30 - yangle) < .1f && Mathf.Abs(zangle - 90) < .1f)
                {
                    transform.eulerAngles = new Vector3(0, 30, 90);
                    rotated = true;
                    rotating = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(xangle, yangle, zangle);
                }
            }

        }

        //transform.position = player.transform.position + offset + yOffset;
        Vector3 targetPos = player.transform.position + offset + yOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, 4 * Time.deltaTime);
    }

}
