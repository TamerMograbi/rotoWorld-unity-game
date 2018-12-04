using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public enum gravityDirection { DOWN, LEFT, RIGHT, UP };
    public Animator anim;
    private Rigidbody rb;
    float speed;
    public float moveVertical;
    public float moveHorizontal;
    float maxJump;
    gravityDirection gravityDir;
    public float angle;
    bool IPressed;
    bool JPressed;
    bool KPressed;
    bool LPressed;
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
    private float finishedTime = .3f;
    private Camera cam;
    GameObject rock;

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

        cldr = GetComponent<BoxCollider>();
        jumpAccel = Physics.gravity.magnitude / 2f;
        gravAccel = Physics.gravity.magnitude;
        rock = Resources.Load<GameObject>("Rock");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        }

        if (Input.GetKeyDown(KeyCode.I) && !JPressed && !KPressed && !LPressed)
        {
            gravityDir = gravityDirection.UP;
            if (!gravityDir.Equals(gravityDirection.UP))
                IPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.J) && !IPressed && !KPressed && !LPressed)
        {
            gravityDir = gravityDirection.LEFT;
            if (!gravityDir.Equals(gravityDirection.LEFT))
                JPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.K) && !JPressed && !IPressed && !LPressed)
        {
            gravityDir = gravityDirection.DOWN;
            if (!gravityDir.Equals(gravityDirection.DOWN))
                KPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.L) && !JPressed && !KPressed && !IPressed)
        {
            gravityDir = gravityDirection.RIGHT;
            if (!gravityDir.Equals(gravityDirection.RIGHT))
                LPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.LeftShift))
            sprinting = true;
        else
            sprinting = false;
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {     
            StartCoroutine(JumpTimer());
            jumping = true;
        }
        else
            jumping = false;

        if(IPressed)
        {
            angle = Mathf.LerpAngle(angle, 180, 8 * Time.deltaTime);
            
            //stop rotation when the character is upside down
            if (Mathf.Abs(transform.rotation.eulerAngles.x - 180) > .1f)
            {
                transform.Rotate(new Vector3(angle * Mathf.Deg2Rad, 0, 0));
            }
            else
            {
                IPressed = false;
                transform.Rotate(new Vector3(180 * Mathf.Deg2Rad, 0, 0));
            }
        }
        if (JPressed)
        {
            angle = Mathf.LerpAngle(angle, -90, 8 * Time.deltaTime);

            //stop rotation when the character is upside down
            if (Mathf.Abs(transform.rotation.eulerAngles.x - -90) > .1f)
            {
                transform.Rotate(new Vector3(angle * Mathf.Deg2Rad, 0, 0));
            }
            else
            {
                JPressed = false;
                transform.Rotate(new Vector3(-90 * Mathf.Deg2Rad, 0, 0));
            }
        }
        if (KPressed)
        {
            angle = Mathf.LerpAngle(angle, 0, 8 * Time.deltaTime);

            //stop rotation when the character is upside down
            if (Mathf.Abs(transform.rotation.eulerAngles.x - 0) > .1f)
            {
                transform.Rotate(new Vector3(angle * Mathf.Deg2Rad, 0, 0));
            }
            else
            {
                KPressed = false;
                transform.Rotate(new Vector3(0 * Mathf.Deg2Rad, 0, 0));
            }
        }
        if (LPressed)
        {
            angle = Mathf.LerpAngle(angle, 90, 8 * Time.deltaTime);

            //stop rotation when the character is upside down
            if (Mathf.Abs(transform.rotation.eulerAngles.x - 90) > .1f)
            {
                transform.Rotate(new Vector3(angle * Mathf.Deg2Rad, 0, 0));
            }
            else
            {
                LPressed = false;
                transform.Rotate(new Vector3(90 * Mathf.Deg2Rad, 0, 0));
            }
        }
    }
    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        moveVertical = Input.GetAxis("Vertical");
        moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement;
        float camAngleY = cam.transform.eulerAngles.y * Mathf.Deg2Rad;
        float camAngleX = cam.transform.eulerAngles.x * Mathf.Deg2Rad;
        //print(camAngleX);
        //print(camAngleY);
        if (gravityDir.Equals(gravityDirection.DOWN)) movement = new Vector3(moveHorizontal * Mathf.Cos(camAngleY) + moveVertical * Mathf.Sin(camAngleY), 0.0f, moveVertical * Mathf.Cos(camAngleY) - moveHorizontal * Mathf.Sin(camAngleY));   // down = normal
        else if (gravityDir.Equals(gravityDirection.UP)) movement = new Vector3(-moveHorizontal * Mathf.Cos(camAngleY) + moveVertical * Mathf.Sin(camAngleY), 0.0f, moveVertical * Mathf.Cos(camAngleY) + moveHorizontal * Mathf.Sin(camAngleY));   // up = reversed left and right
        else if (gravityDir.Equals(gravityDirection.LEFT)) movement = new Vector3(0.0f, -moveHorizontal * Mathf.Cos(camAngleX) - moveVertical * Mathf.Sin(camAngleX), moveVertical * Mathf.Cos(camAngleX) + moveHorizontal * Mathf.Sin(camAngleX));
        else movement = new Vector3(0.0f, moveHorizontal * Mathf.Cos(camAngleX) - moveVertical * Mathf.Sin(camAngleX), moveVertical * Mathf.Cos(camAngleX) - moveHorizontal * Mathf.Sin(camAngleX));
        /*if (gravityDir.Equals(gravityDirection.DOWN)) movement = new Vector3(moveHorizontal, 0.0f, moveVertical);   // down = normal
        else if (gravityDir.Equals(gravityDirection.UP)) movement = new Vector3(-moveHorizontal, 0.0f, moveVertical);   // up = reversed left and right
        else if (gravityDir.Equals(gravityDirection.LEFT)) movement = new Vector3(0.0f, -moveHorizontal, moveVertical);
        else movement = new Vector3(0.0f, moveHorizontal, moveVertical);*/
        if (sprinting)
            movement = movement * 2;

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
                //anim.Play("run");
                if (!sprinting)
                    anim.SetInteger("state", 1);
                else
                    anim.SetInteger("state", 2);
            }
            else
            {
                anim.SetInteger("state", 0);
                //anim.Play("idle");
            }
            //}
            //else
            //{
            //    anim.Play("jump-float");
            //}
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
            if (IsGrounded())
            {
                if (jumping && canJump)
                    rb.AddForce(getJumpDirectionVector() * (sprinting ? jumpAccel * 1.5f : jumpAccel), ForceMode.Impulse);
            }
            rb.AddForce(getCurrentGravity());
        }
        else
        {
            anim.SetInteger("state", 3);
        }

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
        finishedTime = .3f;
        canJump = true;
    }

    // Checks if player is on the ground (currently only works if gravity is pointing down) - Justin
    // https://www.youtube.com/watch?v=vdOFUFMiPDU - video I used as a jumping tutorial
    private bool IsGrounded()
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
        int xForceDir = rnd.Next(-1,1);
        int zForceDir = rnd.Next(-1, 1);
        Vector3 v = getJumpDirectionVector();//getDirectionVector();
        v.x = 3 * xForceDir;
        v.z = 3 * zForceDir;
        if(v.x == 0 && v.z == 0)
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

}
