using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerMouse : MonoBehaviour {

    public GameObject player;
    AnimController animCtrl;
    public AnimController.gravityDirection gravityDir, prevGravityDir;
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

    /*--------- Mouse Camera --------*/
    public Transform lookAt;
    public Transform camTransform;
    private Camera cam;
    private float distance = 10;
    private float currX = 0.0f;
    private float currY = 0.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 1.0f;
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 60.0f;
    private const float MIN_DISTANCE = 3.0f;
    private const float MAX_DISTANCE = 6.0f;

    void Start () {
        player = GameObject.Find("Player");
        animCtrl = player.GetComponent<AnimController>();
        rotated = false;
        rotating = false;
        xangle = 30;
        yangle = 0;
        zangle = 0;
        prevGravityDir = AnimController.gravityDirection.DOWN;

        // mouse camera
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        if (!rotating)
        {
            if (gravityDir == AnimController.gravityDirection.DOWN)
            {
                currX += Input.GetAxis("Mouse X") * sensitivityX;
                currY += Input.GetAxis("Mouse Y") * sensitivityY;
                currY = Mathf.Clamp(currY, Y_ANGLE_MIN, Y_ANGLE_MAX);
            }
            else if (gravityDir == AnimController.gravityDirection.UP)
            {
                currX -= Input.GetAxis("Mouse X") * sensitivityX;
                currY -= Input.GetAxis("Mouse Y") * sensitivityY;
                currY = Mathf.Clamp(currY, Y_ANGLE_MIN, -Y_ANGLE_MAX);
            }
            else if (gravityDir == AnimController.gravityDirection.LEFT)
            {
                currY -= Input.GetAxis("Mouse X") * sensitivityX;
                currX += Input.GetAxis("Mouse Y") * sensitivityY;
                currX = Mathf.Clamp(currX, Y_ANGLE_MIN, Y_ANGLE_MAX);
            }
            else if (gravityDir == AnimController.gravityDirection.RIGHT)
            {
                currY += Input.GetAxis("Mouse X") * sensitivityX;
                currX -= Input.GetAxis("Mouse Y") * sensitivityY;
                currX = Mathf.Clamp(currY, Y_ANGLE_MIN, -Y_ANGLE_MAX);
            }

            distance -= Input.GetAxis("Mouse ScrollWheel");

            distance = Mathf.Clamp(distance, MIN_DISTANCE, MAX_DISTANCE);
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        if(animCtrl.getGravityDir() != gravityDir)
        {
            prevGravityDir = gravityDir;
            gravityDir = animCtrl.getGravityDir();
            rotated = false;
        }
        else if (!rotating)
        {
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currY, currX, zangle);
            camTransform.position = lookAt.position + rotation * dir;
        }
        /*if (gravityDir == AnimController.gravityDirection.UP)
        {
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, -30, 3 * Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, 0, 3 * Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, 180, 3 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(-30 - xangle) < .1f && Mathf.Abs(0 - yangle) < .1f && Mathf.Abs(zangle - 180) < .1f)
                {
                    camTransform.eulerAngles = new Vector3(-30, 0, 180);
                    rotated = true;
                    rotating = false;
                }
                else
                {
                    camTransform.eulerAngles = new Vector3(xangle, yangle, zangle);
                }
            }

        }
        else if (gravityDir == AnimController.gravityDirection.LEFT)
        {
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, 0, Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, -30, Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, -90, Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(0 - transform.rotation.eulerAngles.x) < .1f && Mathf.Abs(-30 - transform.rotation.eulerAngles.y) < .1f && Mathf.Abs(transform.rotation.eulerAngles.z - -90) < .1f)
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
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, 30, Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, 0, Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, 0, Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(30 - transform.rotation.eulerAngles.x) < .1f && Mathf.Abs(0 - transform.rotation.eulerAngles.y) < .1f && Mathf.Abs(transform.rotation.eulerAngles.z - 0) < .1f)
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
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, 0, Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, 30, Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, 90, 2 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(0 - transform.rotation.eulerAngles.x) < .1f && Mathf.Abs(30 - transform.rotation.eulerAngles.y) < .1f && Mathf.Abs(transform.rotation.eulerAngles.z - 90) < .1f)
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

        }*/

        camTransform.LookAt(lookAt.position);
        //transform.position = player.transform.position + offset + yOffset;
        //Vector3 targetPos = player.transform.position + offset + yOffset;
        //transform.position = Vector3.Lerp(transform.position, targetPos, 4 * Time.deltaTime);
    }

}
