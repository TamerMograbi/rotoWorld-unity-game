using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimController : MonoBehaviour
{
    public enum gravityDirection { DOWN, LEFT, RIGHT, UP };
    public enum animState { IDLE = 0, RUN, SPRINT, THROW, JUMP }
    public Animator anim;
    private Rigidbody rb;
    float speed;
    public float moveVertical;
    public float moveHorizontal;
    float maxJump;
    gravityDirection gravityDir;
    public float angle;
    public bool IPressed;
    public bool JPressed;
    public bool KPressed;
    public bool LPressed;
    private bool sprinting;

    public LayerMask groundLayers;
    public BoxCollider cldr;
    private float jumpAccel;
    private float gravAccel;
    public float verticalVelocity = 0.0f;
    private Vector3[] gravVectors = { new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(-1, 0, 0), new Vector3(1, 0, 0) }; // Down, Up, Left, Right
    private bool jumping = false;
    public bool isGrounded = false;
    private bool canJump = true;
    private float finishedTime = .2f;
    private Camera cam;
    GameObject rock;
    float gravityRotSpeed;
    string sceneName;
    

    //-------------- Throwing -------------
    private bool throwing = false;
    private float throwTime = 1.0f;

    // Use this for initialization
    void Start()
    {
        //Time.timeScale = .25f;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        speed = 2;
        moveVertical = 0;
        moveHorizontal = 0;
        maxJump = 8;
        rb.useGravity = false;
        gravityDir = gravityDirection.DOWN;
        angle = 0;
        IPressed = false;
        JPressed = false;
        KPressed = false;
        LPressed = false;
        sprinting = false;
        gravityRotSpeed = 0.05f;

        cldr = GetComponent<BoxCollider>();
        jumpAccel = Physics.gravity.magnitude / 2f;
        gravAccel = Physics.gravity.magnitude;
        rock = Resources.Load<GameObject>("Rock");
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    }

    bool isInInvertedRotation()
    {
        Vector3 rotV = transform.rotation.eulerAngles;
        if (((Mathf.Abs(rotV.x - -180) < 2f) || (Mathf.Abs(rotV.x - 180) < 2f)) && Mathf.Abs(rotV.z - 0) < 2f)
        {
            return true;
        }
        if (Mathf.Abs(rotV.x - 0) < 2f && Mathf.Abs(rotV.z - 180) < 2f)
        {
            return true;
        }
        return false;
    }
    bool isNormalRotation()
    {
        Vector3 rotV = transform.rotation.eulerAngles;
        return Mathf.Abs(rotV.x - 0) < 2f && Mathf.Abs(rotV.z - 0) < 2f;
    }
    bool isLeftRotation()
    {
        Vector3 rotV = transform.rotation.eulerAngles;
        Debug.Log("y = " + rotV.y + " z = " + rotV.z);
        return Mathf.Abs(rotV.y - 0) < 2f && ((Mathf.Abs(rotV.z - -90) < 2f) || (Mathf.Abs(rotV.z - 270) < 2f));
    }
    bool isRightRotation()
    {
        Vector3 rotV = transform.rotation.eulerAngles;
        return Mathf.Abs(rotV.y - 0) < 2f && Mathf.Abs(rotV.z - 90) < 2f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level0");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TrainingLevel");
        }

        if (Input.GetKeyDown(KeyCode.I) && !JPressed && !KPressed && !LPressed)
        {

            if (!gravityDir.Equals(gravityDirection.UP))
            {
                IPressed = true;
            }
            //gravityDir = gravityDirection.UP;
            StartCoroutine(changeGravityTimer(gravityDirection.UP));
        }
        if (Input.GetKeyDown(KeyCode.J) && !IPressed && !KPressed && !LPressed)
        {

            if (!gravityDir.Equals(gravityDirection.LEFT))
            {
                JPressed = true;
            }
            StartCoroutine(changeGravityTimer(gravityDirection.LEFT));
            //gravityDir = gravityDirection.LEFT;
        }
        if (Input.GetKeyDown(KeyCode.K) && !JPressed && !IPressed && !LPressed)
        {

            if (!gravityDir.Equals(gravityDirection.DOWN))
            {
                KPressed = true;
            }
            //gravityDir = gravityDirection.DOWN;
            StartCoroutine(changeGravityTimer(gravityDirection.DOWN));
        }
        if (Input.GetKeyDown(KeyCode.L) && !JPressed && !KPressed && !IPressed)
        {

            if (!gravityDir.Equals(gravityDirection.RIGHT))
            {
                LPressed = true;
            }
            //gravityDir = gravityDirection.RIGHT;
            StartCoroutine(changeGravityTimer(gravityDirection.RIGHT));

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.LeftShift))
            sprinting = true;
        else
            sprinting = false;
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            StartCoroutine(JumpTimer());
            jumping = true;
        }
        else
            jumping = false;

        if (IPressed)
        {
            Vector3 movement = new Vector3(-1, 0, 1);
            Vector3 upVector = new Vector3(0, -1, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement, upVector), gravityRotSpeed);
            if (isInInvertedRotation())
            {
                IPressed = false;
            }
        }
        if (JPressed)
        {
            Vector3 movement = new Vector3(0, -1, 1);
            Vector3 upVector = new Vector3(1, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement, upVector), gravityRotSpeed);
            if (isLeftRotation())
            {
                JPressed = false;
            }
        }
        if (KPressed)
        {
            Vector3 movement = new Vector3(1, 0, 1);
            Vector3 upVector = new Vector3(0, 1, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement, upVector), gravityRotSpeed);
            if (isNormalRotation())
            {
                KPressed = false;
            }
        }
        if (LPressed)
        {
            Vector3 movement = new Vector3(0, 1, 1);
            Vector3 upVector = new Vector3(-1, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement, upVector), gravityRotSpeed);
            if (isRightRotation())
            {
                LPressed = false;
            }

        }

       
    }
    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        moveVertical = Input.GetAxis("Vertical");
        moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement;
        /*float camAngleY = cam.transform.eulerAngles.y * Mathf.Deg2Rad;
        float camAngleX = cam.transform.eulerAngles.x * Mathf.Deg2Rad;*/
        float camAngleY = cam.GetComponent<CameraControllerMouse>().camEulerAngles.y * Mathf.Deg2Rad;
        //print(camAngleX);
        //print(camAngleY);
        /*if (gravityDir.Equals(gravityDirection.DOWN)) movement = new Vector3(moveHorizontal * Mathf.Cos(camAngleY) + moveVertical * Mathf.Sin(camAngleY), 0.0f, moveVertical * Mathf.Cos(camAngleY) - moveHorizontal * Mathf.Sin(camAngleY));   // down = normal
        else if (gravityDir.Equals(gravityDirection.UP)) movement = new Vector3(-moveHorizontal * Mathf.Cos(camAngleY) + moveVertical * Mathf.Sin(camAngleY), 0.0f, moveVertical * Mathf.Cos(camAngleY) + moveHorizontal * Mathf.Sin(camAngleY));   // up = reversed left and right
        else if (gravityDir.Equals(gravityDirection.LEFT)) movement = new Vector3(0.0f, -moveHorizontal * Mathf.Cos(camAngleX) - moveVertical * Mathf.Sin(camAngleX), moveVertical * Mathf.Cos(camAngleX) + moveHorizontal * Mathf.Sin(camAngleX));
        else movement = new Vector3(0.0f, moveHorizontal * Mathf.Cos(camAngleX) - moveVertical * Mathf.Sin(camAngleX), moveVertical * Mathf.Cos(camAngleX) - moveHorizontal * Mathf.Sin(camAngleX));*/
        if (gravityDir.Equals(gravityDirection.DOWN)) movement = new Vector3(moveHorizontal * Mathf.Cos(camAngleY) + moveVertical * Mathf.Sin(camAngleY), 0.0f, moveVertical * Mathf.Cos(camAngleY) - moveHorizontal * Mathf.Sin(camAngleY));   // down = normal
        else if (gravityDir.Equals(gravityDirection.UP)) movement = new Vector3(-moveHorizontal * Mathf.Cos(camAngleY) - moveVertical * Mathf.Sin(camAngleY), 0.0f, moveVertical * Mathf.Cos(camAngleY) - moveHorizontal * Mathf.Sin(camAngleY));   // up = reversed left and right
        else if (gravityDir.Equals(gravityDirection.LEFT)) movement = new Vector3(0.0f, -moveHorizontal * Mathf.Cos(camAngleY) - moveVertical * Mathf.Sin(camAngleY), moveVertical * Mathf.Cos(camAngleY) - moveHorizontal * Mathf.Sin(camAngleY));
        else movement = new Vector3(0.0f, moveHorizontal * Mathf.Cos(camAngleY) + moveVertical * Mathf.Sin(camAngleY), moveVertical * Mathf.Cos(camAngleY) - moveHorizontal * Mathf.Sin(camAngleY));
        if (sprinting)
        {
            movement = movement * 2;
        }

        if (!throwing)
        {
            if (moveVertical != 0 || moveHorizontal != 0)
            {
                Vector3 upVector;
                if (gravityDir == gravityDirection.UP)
                {
                    upVector = new Vector3(0, -1, 0);
                }
                else if (gravityDir == gravityDirection.DOWN)
                {
                    upVector = new Vector3(0, 1, 0);
                }
                else if (gravityDir == gravityDirection.RIGHT)
                {
                    upVector = new Vector3(-1, 0, 0);
                }
                else//LEFT
                {
                    upVector = new Vector3(1, 0, 0);
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement, upVector), 0.15F);

                if (isGrounded)
                {
                    if (!sprinting)
                        anim.SetInteger("state", (int)animState.RUN);
                    else
                        anim.SetInteger("state", (int)animState.SPRINT);
                }
                else//mid-air
                {
                    anim.SetInteger("state", (int)animState.JUMP);
                }
            }
            else
            {
                if (isGrounded)
                {
                    anim.SetInteger("state", (int)animState.IDLE);
                }
                else//mid-air
                {
                    anim.SetInteger("state", (int)animState.JUMP);
                }

            }
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
            if (IsGrounded())
            {
                if (jumping && canJump)
                {
                    FindObjectOfType<AudioManager>().Play("jump");
                    rb.AddForce(getJumpDirectionVector() * (sprinting ? jumpAccel * 1.5f : jumpAccel), ForceMode.Impulse);
                }
            }
            rb.AddForce(getCurrentGravity());
        }
        else
        {
            anim.SetInteger("state", (int)animState.THROW);
        }

        /*if (!cam.GetComponent<CameraControllerMouse>().rotating)
        {
            if (gravityDir == gravityDirection.DOWN)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
            else if (gravityDir == gravityDirection.UP)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 180);
            }
            /*else if (gravityDir == gravityDirection.LEFT)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
            }
            else if (gravityDir == gravityDirection.RIGHT)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 90);
            }
        }*/
        transform.localScale = new Vector3(1, 1, 1);

        // -------- Throw Rock -----------
        if (Input.GetKeyDown(KeyCode.LeftControl) && anim.GetInteger("state") == 0 && IsGrounded())
        {
            throwing = true;
            StartCoroutine(ThrowCountdown());
        }
    }

    private Vector3 getCurrentGravity() // Currently Not In Use
    {
        switch (gravityDir)
        {
            case gravityDirection.DOWN:
                {
                    return new Vector3(0, -Physics.gravity.magnitude, 0);
                }
            case gravityDirection.UP:
                {
                    return new Vector3(0, Physics.gravity.magnitude, 0);
                }
            case gravityDirection.LEFT:
                {
                    return new Vector3(-Physics.gravity.magnitude, 0, 0);
                }
            default://RIGHT
                {
                    return new Vector3(Physics.gravity.magnitude, 0, 0);
                }

                //still need cases for front and back
        }
    }

    private Vector3 getJumpDirectionVector()
    {
        switch (gravityDir)
        {
            case gravityDirection.DOWN:
                return gravVectors[0];
            case gravityDirection.UP:
                return gravVectors[1];
            case gravityDirection.LEFT:
                return gravVectors[3];
            default://RIGHT
                return gravVectors[2];
        }

    }

    public IEnumerator JumpTimer()
    {
        while (finishedTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            finishedTime -= 0.1f;
        }
        finishedTime = .2f;
        canJump = true;
    }

    public IEnumerator changeGravityTimer(gravityDirection dir)
    {
        if (isGrounded && dir != gravityDir)
        {
            rb.AddForce(getJumpDirectionVector()*6, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f);
            FindObjectOfType<AudioManager>().Play("warp");
        }
        gravityDir = dir;
    }

    // Checks if player is on the ground (currently only works if gravity is pointing down) - Justin
    // https://www.youtube.com/watch?v=vdOFUFMiPDU - video I used as a jumping tutorial
    public bool IsGrounded()
    {
        switch (gravityDir)
        {
            case gravityDirection.DOWN:
                return Physics.CheckCapsule(cldr.bounds.center, new Vector3(cldr.bounds.center.x, cldr.bounds.min.y, cldr.bounds.center.z), Mathf.Min(cldr.size.x, cldr.size.z) / 2 * .25f, groundLayers);
            case gravityDirection.UP:
                return Physics.CheckCapsule(cldr.bounds.center, new Vector3(cldr.bounds.center.x, cldr.bounds.max.y, cldr.bounds.center.z), Mathf.Min(cldr.size.x, cldr.size.z) / 2 * .25f, groundLayers);
            case gravityDirection.LEFT:
                return Physics.CheckCapsule(cldr.bounds.center, new Vector3(cldr.bounds.min.x, cldr.bounds.center.y, cldr.bounds.center.z), Mathf.Min(cldr.size.x, cldr.size.z) / 2 * .25f, groundLayers);
            default://RIGHT
                return Physics.CheckCapsule(cldr.bounds.center, new Vector3(cldr.bounds.max.x, cldr.bounds.center.y, cldr.bounds.center.z), Mathf.Min(cldr.size.x, cldr.size.z) / 2 * .25f, groundLayers);

                //still need cases for front and back
        }

    }

    public gravityDirection getGravityDir()
    {
        return gravityDir;
    }

    public void takeDamage()
    {
        System.Random rnd = new System.Random();
        //add a bit of randomness in which director character is forced
        int xForceDir = rnd.Next(-1, 1);
        int zForceDir = rnd.Next(-1, 1);
        Vector3 v = getJumpDirectionVector();//getDirectionVector();
        v.x = 3 * xForceDir;
        v.z = 3 * zForceDir;
        if (v.x == 0 && v.z == 0)
        {
            v.x = 3;
        }
        v.y *= 3;

        rb.AddForce(v, ForceMode.Impulse);
    }

    public IEnumerator ThrowCountdown()
    {
        bool thrown = false;
        while (throwTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            throwTime -= 0.1f;
            if ((throwTime - .2f) < .01f && !thrown)
            {
                GameObject.Instantiate(rock, new Vector3(transform.position.x, transform.position.y + 1, transform.position.x), new Quaternion());
                thrown = true;
            }
        }
        throwing = false;
        throwTime = 1.0f;
    }

    public float getHorizontalMovement()
    {
        return moveHorizontal;
    }
    public float getVerticalMovement()
    {
        return moveVertical;
    }
    public float getSpeed()
    {
        return speed;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("letter"))
        {
            if(sceneName == "Level1")//just completed level 1
            {
                GlobalCtrl.instance.levelsCompleted[0] = true;
                GlobalCtrl.instance.dungeonMasterState = 1;
            }
            else if(sceneName == "Level2")//just completed level 2
            {
                GlobalCtrl.instance.levelsCompleted[1] = true;
            }
            else if(sceneName == "TrainingLevel")
            {
                GlobalCtrl.instance.levelsCompleted[2] = true;
            }
        }

        if (other.gameObject.CompareTag("Death Box"))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
