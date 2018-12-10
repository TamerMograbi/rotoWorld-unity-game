﻿using System.Collections;
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
    public Vector3 camEulerAngles;
    private Camera cam;
    public GameObject mockCamera;
    private float distance = 10;
    private float collisionDistance = -1;
    private float camDistance = 10;
    public float currX = 0.0f;
    public float currY = 0.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 1.0f;
    private const float Y_ANGLE_MIN = -30.0f;
    private const float Y_ANGLE_MAX = 60.0f;
    private const float MIN_DISTANCE = 3.0f;
    private const float MAX_DISTANCE = 6.0f;

    public Vector3 change;
    public Vector3 change2;

    /*public CollisionHandler collision = new CollisionHandler();
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero;*/

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

        // collision detection
        /*collision.Initialize(cam);
        collision.UpdateClipPoints(transform.position, transform.rotation, ref collision.adjustedClipPoints);
        collision.UpdateClipPoints(destination, transform.rotation, ref collision.desiredClipPoints);*/
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

        /*collision.CheckColliding(camTransform.position);
        collisionDistance = collision.GetAdjustedDistance(camTransform.position);
        if (collision.colliding)
            camDistance = collisionDistance * distance;
        else*/
        camDistance = distance;

        float lockY = -999.0f;
        float lockX = -999.0f;
        if (currY < 5.0f)
        {
            if (gravityDir == AnimController.gravityDirection.DOWN)
            {
                lockY = transform.position.y;
                lookAt.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + -(currY - 5.0f) / 10, player.transform.position.z);
            }
            if (gravityDir == AnimController.gravityDirection.UP)
            {
                lockY = transform.position.y;
                lookAt.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - -(currY - 5.0f) / 10, player.transform.position.z);
            }
            if (gravityDir == AnimController.gravityDirection.LEFT)
            {
                lockX = transform.position.x;
                lookAt.transform.position = new Vector3(player.transform.position.x + -(currY - 5.0f) / 10, player.transform.position.y, player.transform.position.z);
            }
            if (gravityDir == AnimController.gravityDirection.RIGHT)
            {
                lockX = transform.position.x;
                lookAt.transform.position = new Vector3(player.transform.position.x - -(currY - 5.0f) / 10, player.transform.position.y, player.transform.position.z);
            }
            //currY = 5.0f;
        }

        Vector3 dir = new Vector3(0, 0, -camDistance);
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
        mockCamera.transform.position = Vector3.zero + Quaternion.Euler(currY, currX, 0) * dir;
        if (lockY != -999.0f)
        {
            camTransform.position = new Vector3(lockX == -999.0f ? camTransform.position.x : Mathf.Clamp(camTransform.position.x, lockX, lockX), lockY == -999.0f ? camTransform.position.y : Mathf.Clamp(camTransform.position.y, lockY, lockY), camTransform.position.z);
        }

        if (gravityDir == AnimController.gravityDirection.UP)
        {
            if (!rotated)
            {
                rotating = true;
                xangle = Mathf.LerpAngle(xangle, -30, 3 * Time.deltaTime);
                yangle = Mathf.LerpAngle(yangle, 0, 3 * Time.deltaTime);
                zangle = Mathf.LerpAngle(zangle, 180, 3 * Time.deltaTime);

                //stop rotation when the character is upside down
                if (/*Mathf.Abs(-30 - xangle) < .1f && Mathf.Abs(0 - yangle) < .1f &&*/ Mathf.Abs(zangle - 180) < .1f || Mathf.Abs(zangle - -180) < .1f)
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
                if (/*Mathf.Abs(0 - xangle) < .1f && Mathf.Abs(-30 - yangle) < .1f &&*/ (Mathf.Abs(zangle - -90) < .1f) || Mathf.Abs(zangle - 270) < .1f)
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
                if (/*Mathf.Abs(30 - xangle) < .1f && Mathf.Abs(0 - yangle) < .1f &&*/ (Mathf.Abs(zangle - 0) < .1f) || (Mathf.Abs(zangle - 360) < .1f))
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
                if (/*Mathf.Abs(0 - xangle) < .1f && Mathf.Abs(30 - yangle) < .1f &&*/ Mathf.Abs(zangle - 90) < .1f || Mathf.Abs(zangle - -270) < .1f)
                {
                    zangle = 90;
                    rotated = true;
                    rotating = false;
                }
            }

        }

        mockCamera.transform.LookAt(Vector3.zero);
        camEulerAngles = mockCamera.transform.eulerAngles;
        camTransform.LookAt(lookAt.position, new Vector3(-Mathf.Sin(zangle * Mathf.Deg2Rad), Mathf.Cos(zangle * Mathf.Deg2Rad), 0));
        
        //camTransform.LookAt(lookAt.position, new Vector3(1, 0, 0));
        //transform.position = player.transform.position + offset + yOffset;
        //Vector3 targetPos = player.transform.position + offset + yOffset;
        //transform.position = Vector3.Lerp(transform.position, targetPos, 4 * Time.deltaTime);
    }


    // Camera collision with environment 
    //https://www.youtube.com/watch?v=Uqi2jEgvVsI
    /*[System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedClipPoints;
        [HideInInspector]
        public Vector3[] desiredClipPoints;

        Camera camera;

        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedClipPoints = new Vector3[5];
            desiredClipPoints = new Vector3[5];
        }

        private bool CollisionAtClipPoints(Vector3[] clipPoints, Vector3 fromPos)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPos, clipPoints[i] - fromPos);
                float rayDist = Vector3.Distance(clipPoints[i], fromPos);
                if (Physics.Raycast(ray, rayDist, collisionLayer))
                    return true;
                
            }
            return false;
        }

        public void UpdateClipPoints(Vector3 camPos, Quaternion atRotation, ref Vector3[] arr)
        {
            arr = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
            float y = x / camera.aspect;

            // top left
            arr[0] = (atRotation * new Vector3(-x, y, z)) + camPos;
            // top right
            arr[1] = (atRotation * new Vector3(x, y, z)) + camPos;
            // bot left
            arr[2] = (atRotation * new Vector3(-x, -y, z)) + camPos;
            // bot right
            arr[3] = (atRotation * new Vector3(x, -y, z)) + camPos;
            // cam position
            arr[4] = camPos - camera.transform.forward;
        }

        public float GetAdjustedDistance(Vector3 from)
        {
            float dist = 1;

            for (int i = 0; i < desiredClipPoints.Length; i++)
            {
                Ray ray = new Ray(from, desiredClipPoints[i] - from);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (dist == 1)
                        dist = hit.distance;
                    else
                    {
                        if (hit.distance < dist)
                            dist = hit.distance;
                    }
                }
            }

            if (dist == 1)
                return 0;
            else
                return dist;
        }

        public void CheckColliding(Vector3 targetPos)
        {
            if (CollisionAtClipPoints(desiredClipPoints, targetPos))
                colliding = true;
            else
                colliding = false;
        }
    }*/
}
