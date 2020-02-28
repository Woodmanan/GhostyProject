using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    private bool ghostMode = false;
    private float accel = 7;
    private float maxSpeed = 100;
    private float currSpeed = 20;
    private float jumpValue = 100;
    private bool isJumping = false; // checks if accepting vertical jump input
    private Rigidbody2D thisBody;

    private Vector2 direction = Vector2.right;

    private float airDuration = 0;  //duration in air
    private float airDurationLimit = 6;  //essentially jump height limit
  

    private bool grounded = false;
    private int debugNum = 0;


    private List<string> floors = new List<string>() { "Floor", "Platform"};

    // Start is called before the first frame update
    void Start()
    {
        
        thisBody = GetComponent<Rigidbody2D>();
        thisBody.freezeRotation = true;
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetButtonUp("Jump"))
        {

           

            if (ghostMode == true)
            {
                ghostMode = false;
                Ghost theGhost = transform.Find("Ghost").GetComponent<Ghost>();
                theGhost.ghostMode = false;

                Transform myChild = transform.Find("Ghost");
                myChild.localPosition = new Vector3(0, 0, 0);
                

            }
            else
            {
                ghostMode = true;
                Ghost myChild = transform.GetChild(1).GetComponent<Ghost>();
                myChild.ghostMode = true;
            }

            debugNum++;
        }


    }
    void FixedUpdate()
    {
         
         if (!ghostMode)
         {
             Movement();
         }
         
       

    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (floors.Contains(collision.gameObject.name))
        {
            grounded = true;
            isJumping = false;
        }
    }

    private void Movement()
    {

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {

            float hMove = Input.GetAxisRaw("Horizontal") * currSpeed;
            float vMove = 0;
            float jump = Input.GetAxisRaw("Vertical") * jumpValue;

            if ((jump > 0) && grounded)
            {

                isJumping = true;
                grounded = false;
            }

            if (isJumping)
            {
                vMove = jump;
            }



            Vector2 directionMoved = new Vector2(hMove, vMove);
            thisBody.AddForce(directionMoved);


        }

        if (isJumping)
        {
            airDuration += 1;
        }
        else
        {
            airDuration = 0;
        }

        if (airDuration > airDurationLimit)
        {

            isJumping = false;

        }


    }

   

  


}
