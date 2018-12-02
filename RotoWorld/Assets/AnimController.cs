using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    enum gravityDirection { DOWN, LEFT, RIGHT, UP };
    public Animator anim;
    private Rigidbody rb;
    float speed;
    public float moveVertical;
    public float moveHorizontal;
    float maxJump;
    gravityDirection gravityDir;
    public float angle;
    bool CtrlPressed;

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

    // Use this for initialization
    void Start()
    {
        //Time.timeScale = .25f;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        speed = 2;
        moveVertical = 0;
        moveHorizontal = 0;
        maxJump = 8;
        rb.useGravity = false;
        gravityDir = gravityDirection.DOWN;
        angle = 0;
        CtrlPressed = false;

        cldr = GetComponent<BoxCollider>();
        jumpAccel = Physics.gravity.magnitude / 1.5f;
        gravAccel = Physics.gravity.magnitude;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            gravityDir = gravityDirection.UP;
            CtrlPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {     
            StartCoroutine(JumpTimer());
            jumping = true;
        }
        else
            jumping = false;

        //We can't have this code in the above if as lerp needs to keep working also after the key space gets pressed and only stop
        //when a 180 degrees rotation has been reached.
        if(CtrlPressed)
        {
            angle = Mathf.LerpAngle(angle, 180, 8 * Time.deltaTime);
            
            //stop rotation when the character is upside down
            if (transform.rotation.eulerAngles.x < 180)
            {
                transform.Rotate(new Vector3(angle * Mathf.Deg2Rad, 0, 0));
            }
            else
            {
                CtrlPressed = false;
            }
        }
    }
    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        moveVertical = Input.GetAxis("Vertical");
        moveHorizontal = Input.GetAxis("Horizontal");

        // Adjust inputs based on which wall you're on
        Vector3 movement = new Vector3();
        if (gravityDir.Equals(gravityDirection.DOWN)) movement = new Vector3(moveHorizontal, 0.0f, moveVertical);   // down = normal
        else if (gravityDir.Equals(gravityDirection.UP)) movement = new Vector3(-moveHorizontal, 0.0f, moveVertical);   // up = reversed left and right
        else if (gravityDir.Equals(gravityDirection.LEFT)) movement = new Vector3(0.0f, -moveHorizontal, moveVertical);
        else movement = new Vector3(0.0f, moveHorizontal, moveVertical);
        if (moveVertical != 0 || moveHorizontal != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
            anim.Play("run");
        }
        else
        {
            anim.Play("idle");
        }
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        if (IsGrounded())
        {
            /*verticalVelocity = -gravAccel * Time.deltaTime;
            if (jumping && canJump)
            {
                verticalVelocity = jumpAccel;
                jumping = false;
                canJump = false;
            }*/
            if (jumping && canJump)
                rb.AddForce(getJumpDirectionVector() * jumpAccel, ForceMode.Impulse);
        }
        /*else
            verticalVelocity -= gravAccel * Time.deltaTime;

        rb.AddForce(getDirectionVector() * verticalVelocity);*/
        rb.AddForce(getCurrentGravity());
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
                return gravVectors[2];
            default://RIGHT
                return gravVectors[3];

            //still need cases for front and back
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

}
