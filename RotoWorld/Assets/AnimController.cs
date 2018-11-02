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
    bool SpacePressed;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        speed = 2;
        moveVertical = 0;
        moveHorizontal = 0;
        maxJump = 8;
        rb.useGravity = false;
        gravityDir = gravityDirection.DOWN;
        angle = 0;
        SpacePressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gravityDir = gravityDirection.UP;
            SpacePressed = true;
        }

        //We can't have this code in the above if as lerp needs to keep working also after the key space gets pressed and only stop
        //when a 180 degrees rotation has been reached.
        if(SpacePressed)
        {
            angle = Mathf.LerpAngle(angle, 180, 8 * Time.deltaTime);
            
            //stop rotation when the character is upside down
            if (transform.rotation.eulerAngles.x < 180)
            {
                transform.Rotate(new Vector3(angle * Mathf.Deg2Rad, 0, 0));
            }
            else
            {
                SpacePressed = false;
            }
        }
    }
    private void FixedUpdate()
    {
        moveVertical = Input.GetAxis("Vertical");
        moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
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
        rb.AddForce(getCurrentGravity());
    }

    private Vector3 getCurrentGravity()
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
        }

    }

}
