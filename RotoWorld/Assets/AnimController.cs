using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{

    public Animator anim;
    private Rigidbody rb;
    float speed;
    public float moveVertical;
    public float moveHorizontal;
    float maxJump;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        speed = 2;
        moveVertical = 0;
        moveHorizontal = 0;
        maxJump = 8;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
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
        
    }
}
