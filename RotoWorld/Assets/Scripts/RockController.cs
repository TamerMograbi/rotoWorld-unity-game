using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour {

    AnimController animController;
    Camera cam;
    GameObject player;
    public AnimController.gravityDirection gravityDir;
    private float hspeed;
    private float vspeed;
    private float gravAccel;
    private float camAngleX;
    private float camAngleY;
    private float camAngleZ;
    private float moveX;
    private float moveY;

    private float lifetime;

    // Use this for initialization
    void Start () {
        animController = GameObject.Find("Player").GetComponent<AnimController>();
        player = GameObject.Find("Player");
        cam = Camera.main;
        hspeed = 6;
        gravityDir = animController.getGravityDir();
        vspeed = .5f * Physics.gravity.magnitude;
        gravAccel = .5f * -Physics.gravity.magnitude;
        camAngleX = cam.transform.eulerAngles.x;
        camAngleY = cam.transform.eulerAngles.y;
        camAngleZ = cam.transform.eulerAngles.z;
        /*camAngleX = player.transform.eulerAngles.x;
        camAngleY = player.transform.eulerAngles.y;
        camAngleZ = player.transform.eulerAngles.z;*/
        transform.localEulerAngles = new Vector3(camAngleX, camAngleY, camAngleZ);
        if (gravityDir == AnimController.gravityDirection.DOWN)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
        else if (gravityDir == AnimController.gravityDirection.UP)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z);
        else if (gravityDir == AnimController.gravityDirection.LEFT)
            transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z);
        else if (gravityDir == AnimController.gravityDirection.UP)
            transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y, player.transform.position.z);
        lifetime = 5.0f;
        StartCoroutine(Countdown());
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0, vspeed * Time.deltaTime, hspeed * Time.deltaTime));
        vspeed += gravAccel * Time.deltaTime;
        // hi :Ps
    }

    public void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.layer);
        if (collision.gameObject.layer == 0) //default
        {
            gravAccel = 0;
            vspeed = 0;
            hspeed -= .5f * hspeed * Time.deltaTime;
            lifetime = .5f;
        }
    }

    public IEnumerator Countdown()
    {
        while (lifetime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            lifetime -= 0.1f;
        }
        Destroy(this.gameObject);
    }
}
