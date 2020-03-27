using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Character : MonoBehaviour
{

    [HideInInspector] public bool ghostMode = false;
    [SerializeField] private float accel = 7;
    [SerializeField] private float maxSpeed = 100;
    [SerializeField] private float jumpForce;
    private float currSpeed = 20;
    private float jumpValue = 100;
    private bool isJumping = false; // checks if accepting vertical jump input
    private Rigidbody2D thisBody;



    private float airDuration = 0;  //duration in air
    [SerializeField] private float jumpCooldownTime;
    private float airDurationLimit = 6;  //essentially jump height limit
    [SerializeField] private LayerMask jumpingMask;
    private float jumpCooldown;
  

    private bool grounded = false;
    private int debugNum = 0;

    private SpriteRenderer sprite;


    private List<string> floors = new List<string>() { "Floor", "Platform"};

    private Ghost ghost;

    [SerializeField] private float radius;
    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private float jumpRad;
    [SerializeField] private int health;

    // Start is called before the first frame update
    void Start()
    {
        thisBody = GetComponent<Rigidbody2D>();
        thisBody.freezeRotation = true;
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        jumpCooldown = 0;
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
    }

    // Update is called once per frame
    
    void Update()
    {
        //Update for every frame
        jumpCooldown -= Time.deltaTime;
        
        
        if (Input.GetButtonDown("Jump"))
        {

           

            if (ghostMode)
            {
                if (ghost.gameObject.activeSelf)
                {
                    //Ghostmode set in ghost now, to better fit animation
                    //ghostMode = false;
                    ghost.disableGhostMode();
                    //thisBody.mass = 1f;
                    thisBody.drag = 0;

                    //Reset position
                    //ghost.transform.localPosition = new Vector3(0, 0, 1);
                }
            }
            else
            {

                ghostMode = true;
                ghost.enableGhostMode();
                //thisBody.mass = 100f;
                thisBody.drag = 4f;

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

    public GameObject GetBeneath()
    {
        //Set up raycast, based on ground check
        RaycastHit2D hit;
        hit = Physics2D.CircleCast(transform.position, jumpRad, groundCheck, groundCheck.magnitude - jumpRad, jumpingMask);
        //hit = Physics2D.Raycast(transform.position, groundCheck, groundCheck.magnitude, jumpingMask);
        Debug.DrawRay(transform.position, groundCheck, Color.yellow, groundCheck.magnitude);
        
        //Check for hit!
        if (hit.collider)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
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

    //Raycasting test, to ensure that we're on the ground
    private bool onGround()
    {
        
        //Set up raycast, based on ground check
        RaycastHit2D hit;
        hit = Physics2D.CircleCast(transform.position, jumpRad, groundCheck, groundCheck.magnitude - jumpRad, jumpingMask);
        //hit = Physics2D.Raycast(transform.position, groundCheck, groundCheck.magnitude, jumpingMask);
        Debug.DrawRay(transform.position, groundCheck, Color.yellow, groundCheck.magnitude);
        
        //Check for hit!
        if (hit.collider)
        {
            print("It's on the ground!");
            grounded = true;
            return true;
        }
        else
        {
            print("Tried to be on the ground, and wasn't!");
            grounded = false;
            return false;
        }
    }

    private void Movement()
    {
        //Rigidbody Velocity Movement
        float inputVelocty = Input.GetAxisRaw("Horizontal");
        thisBody.velocity = new Vector2(inputVelocty * maxSpeed, thisBody.velocity.y);
        
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {

            float hMove = Input.GetAxisRaw("Horizontal") * currSpeed;
            float vMove = 0;
            float jump = Input.GetAxisRaw("Vertical") * jumpValue;
            
            //Uses onGround to raycast for floor collisions
            if ((jump > 0) && onGround() && jumpCooldown <= 0)
            {
                //vMove = jump;
                //Add Force to body to cause jump
                thisBody.velocity = new Vector2(thisBody.velocity.x, 0);
                thisBody.AddForce(jumpForce * Vector2.up, ForceMode2D.Force);
                
                jumpCooldown = jumpCooldownTime;
            }

            //Flip our sprite!
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                sprite.flipX = false;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                sprite.flipX = true;

            }

            /*
            Vector2 directionMoved = new Vector2(hMove, vMove);
            thisBody.AddForce(directionMoved);
            */


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

    public float getDistance()
    {
        return radius;
    }

    public Vector2 getPosition()
    {
        return thisBody.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(groundCheck.x, groundCheck.y, 0));
        Gizmos.DrawWireSphere(transform.position + new Vector3(groundCheck.x, groundCheck.y + jumpRad, 0), jumpRad);
    }


    private void Die()
    {
        GameManager.instance.RestartLevel();
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnDestroy()
    {
        print("Destroying");
        //GetComponent<CapsuleCollider2D>().sharedMaterial.friction = 1;
    }
}
