using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private float speed = 400f;

    private GameObject joyStickObj;
    private Joystick joyStick;

    private GameObject cam, player, UIController;
    private UI UIControllerScript;
    private Transform camTransform;
    private Animator camAni;

    private Rigidbody rb;


    private bool pickUp;

    private float pickUpCount;

    private bool lauch = false;
    private bool huisjeHit;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        UIController = GameObject.Find("UIController");
        UIControllerScript = UIController.gameObject.GetComponent<UI>();

        cam = GameObject.Find("Camera");
        camTransform = cam.gameObject.GetComponent<Transform>();
        camAni = cam.gameObject.GetComponent<Animator>();

        Application.targetFrameRate = 60;
        speed = speed * Time.deltaTime; //houdt snelheid aan per frame
        rb = player.GetComponent<Rigidbody>(); // pakt RB van de player

        pickUp = false;

        pickUpCount = 0;

        Physics.autoSimulation = true;

    }

    private void Awake()
    {
        joyStickObj = GameObject.Find("Fixed Joystick");
        joyStick = joyStickObj.GetComponent<Joystick>();
    }

    void FixedUpdate() // fixedUpdate
    {
        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }

        if (lauch == false)
        {
            float moveHorizontal = Input.GetAxis("Horizontal"); //pakt horizontal axis
            float moveVertical = Input.GetAxis("Vertical"); // pakt vertical axis

            moveHorizontal = joyStick.Horizontal; // defineert de horizontal axis aan de joystick
            moveVertical = joyStick.Vertical; // defineert de vertical axis aan de joystick

            rb.AddForce(moveHorizontal * camTransform.transform.right * speed);
            rb.AddForce(moveVertical * camTransform.transform.forward * speed);

        }


        if (lauch) // als lauch true is dan voor het volgende uit...
        {
            if (pickUp ==  true)
            {
                rb.AddForce(new Vector3(0f, 50f, 800f * pickUpCount* Time.deltaTime));
                Invoke("LauchBool", 0.5f);
            }
            else
            {
                rb.AddForce(new Vector3(0f, 50f, 900f * Time.deltaTime));
                Invoke("LauchBool", 0.2f);
            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            camAni.SetBool("animationStart", false);
            camAni.SetBool("animationRamp", true);
            UIControllerScript.SetTimerUnactive();
        }

        else if (other.gameObject.CompareTag("DoorEnd"))
        {
            lauch = true;
            UIControllerScript.SetJoyStickUnactive();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Huisje"))
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Invoke("TimerCity", 5f);
        }

        if (collision.gameObject.CompareTag("Toy"))
        {
            pickUp = true;
            pickUpCount += 7;
            Destroy(collision.gameObject);
        }
    }

    private void LauchBool()
    {
        lauch = false;
    }

    private void TimerCity()
    {
        if (huisjeHit == false)
        {
            huisjeHit = true;
            UIControllerScript.CityTimerSetActive();
        }
    } 
}
