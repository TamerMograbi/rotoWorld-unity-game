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
    public float zangle = 0.0f;    // same angle as player rotation (0, 90, 180, -90)
    //private float ztarget = 0.0f;
    // Use this for initialization

    /*--------- Mouse Camera --------*/
    public Transform lookAt;
    public Transform camTransform;
    private Camera cam;
    private float distance = 10;
    public float currX = 0.0f;
    public float currY = 0.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 1.0f;
    private const float Y_ANGLE_MIN = 10.0f;
    private const float Y_ANGLE_MAX = 60.0f;
    private const float MIN_DISTANCE = 3.0f;
    private const float MAX_DISTANCE = 6.0f;

    public Vector3 change;
    public Vector3 change2;

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
            /*if (gravityDir == AnimController.gravityDirection.DOWN)
            {
                currX += Input.GetAxis("Mouse X") * sensitivityX;
                currY += Input.GetAxis("Mouse Y") * sensitivityY;
                currY = Mathf.Clamp(currY, Y_ANGLE_MIN, Y_ANGLE_MAX);
            }
            else if (gravityDir == AnimController.gravityDirection.UP)
            {
                currX -= Input.GetAxis("Mouse X") * sensitivityX;
                currY -= Input.GetAxis("Mouse Y") * sensitivityY;
                currY = Mathf.Clamp(currY, -Y_ANGLE_MAX, -Y_ANGLE_MIN);
            }
            else if (gravityDir == AnimController.gravityDirection.LEFT)
            {
                currY += Input.GetAxis("Mouse X") * sensitivityX;
                currX -= Input.GetAxis("Mouse Y") * sensitivityY;
                currX = Mathf.Clamp(currX, -Y_ANGLE_MAX, -Y_ANGLE_MIN);
            }
            else if (gravityDir == AnimController.gravityDirection.RIGHT)
            {
                currY -= Input.GetAxis("Mouse X") * sensitivityX;
                currX += Input.GetAxis("Mouse Y") * sensitivityY;
                currX = Mathf.Clamp(currX, Y_ANGLE_MIN, Y_ANGLE_MAX);
            }*/
            currX += Input.GetAxis("Mouse X") * sensitivityX;
            currY += Input.GetAxis("Mouse Y") * sensitivityY;
            currY = Mathf.Clamp(currY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            distance -= Input.GetAxis("Mouse ScrollWheel");

            distance = Mathf.Clamp(distance, MIN_DISTANCE, MAX_DISTANCE);
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        if (animCtrl.getGravityDir() != gravityDir)
        {
            prevGravityDir = gravityDir;
            gravityDir = animCtrl.getGravityDir();
            rotated = false;
        }
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = new Quaternion();
        Quaternion rotation2 = Quaternion.Euler(0, 0, 0);
        if (gravityDir == AnimController.gravityDirection.DOWN)
        {
            rotation = Quaternion.Euler(currY, currX, 0);
            //change2 = new Vector3(currY, currX, 0);
            change2 = rotation * dir;
            //dir = new Vector3(0, 0, -distance);
        }
        else if (gravityDir == AnimController.gravityDirection.UP)
        {
            rotation = Quaternion.Euler(-currY, -currX, 0);
            //dir = new Vector3(0, 0, -distance);
        }
        else if (gravityDir == AnimController.gravityDirection.LEFT)
        {
            rotation = Quaternion.Euler(currY, currX, 0);
            rotation2 = Quaternion.Euler(0, 0, -90);
            //rotation = Quaternion.Euler(currX, -currY, 0);
            //change2 = new Vector3(currX, -currY, 0);
            change2 = rotation * dir;
        }
        else
        {
            rotation = Quaternion.Euler(currY, currX, 0);
            rotation2 = Quaternion.Euler(0, 0, 90);
            //dir = new Vector3(0, -distance, 0);
        }
        change = rotation.eulerAngles;


        camTransform.position = lookAt.position + rotation2 * (rotation * dir);

        if (gravityDir == AnimController.gravityDirection.UP)
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
                    //camTransform.eulerAngles = new Vector3(-30, 0, 180);
                    zangle = 180;
                    rotated = true;
                    rotating = false;
                }
                else
                {
                    //camTransform.eulerAngles = new Vector3(xangle, yangle, zangle);
                }
            }

        }
        else if (gravityDir == AnimController.gravityDirection.LEFT)
        {
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, 0, 3 * Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, -30, 3 * Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, -90, 3 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(0 - xangle) < .1f && Mathf.Abs(-30 - yangle) < .1f && (Mathf.Abs(zangle - -90) < .1f) || Mathf.Abs(zangle - 270) < .1f)
                {
                    zangle = -90;
                    rotated = true;
                    rotating = false;
                }
            }

        }
        else if (gravityDir == AnimController.gravityDirection.DOWN)
        {
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, 30, 3 * Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, 0, 3 * Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, 0, 3 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(30 - xangle) < .1f && Mathf.Abs(0 - yangle) < .1f && (Mathf.Abs(zangle - 0) < .1f) || (Mathf.Abs(zangle - 360) < .1f))
                {
                    zangle = 0;
                    rotated = true;
                    rotating = false;
                }
            }
        }
        else if (gravityDir == AnimController.gravityDirection.RIGHT)
        {
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, 0, 3 * Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, 30, 3 * Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, 90, 3 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (Mathf.Abs(0 - xangle) < .1f && Mathf.Abs(30 - yangle) < .1f && Mathf.Abs(zangle - 90) < .1f)
                {
                    zangle = 90;
                    rotated = true;
                    rotating = false;
                }
            }

        }

        camTransform.LookAt(lookAt.position, new Vector3(-Mathf.Sin(zangle * Mathf.Deg2Rad), Mathf.Cos(zangle * Mathf.Deg2Rad), 0));
        //camTransform.LookAt(lookAt.position, new Vector3(1, 0, 0));
        //transform.position = player.transform.position + offset + yOffset;
        //Vector3 targetPos = player.transform.position + offset + yOffset;
        //transform.position = Vector3.Lerp(transform.position, targetPos, 4 * Time.deltaTime);
    }

}
