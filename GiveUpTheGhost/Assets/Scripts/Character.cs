﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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

    public AudioClip beGhost;
    public AudioClip beBoy;
    public AudioClip jumpsfx;
    public AudioClip land;
    public AudioClip damagesfx;


    private List<string> floors = new List<string>() { "Floor", "Platform", "RightHand", "LeftHand"};

    private Ghost ghost;

    [SerializeField] private float radius;
    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private float jumpRad;
    [SerializeField] private int health;

    private CapsuleCollider2D capsule;
    private PlayerAnimation pa;

    private PhysicsMaterial2D mat;

    private bool onGroundLast = true;

    // Start is called before the first frame update
    void Start()
    {
        thisBody = GetComponent<Rigidbody2D>();
        thisBody.freezeRotation = true;
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        jumpCooldown = 0;
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        capsule = GetComponent<CapsuleCollider2D>();
        pa = GetComponentInChildren<PlayerAnimation>();
        mat = GetComponent<CapsuleCollider2D>().sharedMaterial;
        setFriction(0);
    }

    // Update is called once per frame
    
    void Update()
    {
        //Update for every frame
        jumpCooldown -= Time.deltaTime;
        
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

           

            if (ghostMode)
            {
                if (ghost.gameObject.activeSelf)
                {
                    //Ghostmode set in ghost now, to better fit animation
                    //ghostMode = false;
                    ghost.disableGhostMode();
                    GetComponent<AudioSource>().PlayOneShot(beBoy);
                    //thisBody.mass = 1f;
                    //thisBody.drag = 0;
                    //setFriction(0);

                    //Reset position
                    //ghost.transform.localPosition = new Vector3(0, 0, 1);
                }
            }
            else
            {

                ghostMode = true;
                ghost.enableGhostMode();
                //thisBody.mass = 100f;
                GetComponent<AudioSource>().PlayOneShot(beGhost);
                //thisBody.drag = 4f;
                //setFriction(.5f);

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
        if (hit.collider && hit.collider.gameObject.layer.Equals(9))
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    
    }


    public void setFriction(float friction)
    {
        friction = Mathf.Clamp(friction, 0, 1);
        capsule.sharedMaterial.friction = friction;
        capsule.enabled = false;
        capsule.enabled = true;
        if (friction == 0)
        {
            thisBody.drag = 0;
        }
        else
        {
            thisBody.drag = 4;
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
            //print("It's on the ground!");
            grounded = true;
            return true;
        }
        else
        {
            //print("Tried to be on the ground, and wasn't!");
            grounded = false;
            return false;
        }
    }

    private void Movement()
    {
        // Handle landing if applicable
        if (onGround() && !onGroundLast)
        {
            GetComponent<AudioSource>().PlayOneShot(land, 1f);
            // The player just landed, handle animation and relinquish control
            if (pa.facing) pa.SetAnimation("rightLand");
            else pa.SetAnimation("leftLand");
            onGroundLast = true;
            thisBody.velocity = new Vector2(0f, 0f);
            return;
        }
        onGroundLast = onGround();
        if (pa.landing) return;

        //Rigidbody Velocity Movement
        float inputVelocty = Input.GetAxisRaw("Horizontal");
        // Only apply horizontal velocity if not mid-turn
        if (pa.turning) inputVelocty = 0f;
        thisBody.velocity = new Vector2(inputVelocty * maxSpeed, thisBody.velocity.y);
        
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Jump") != 0)
        {

            float hMove = Input.GetAxisRaw("Horizontal") * currSpeed;
            float vMove = 0;
            float jump = (Input.GetAxisRaw("Vertical") + Input.GetAxis("Jump")) * jumpValue ;

            //Uses onGround to raycast for floor collisions
            if ((jump > 0) && onGround() && jumpCooldown <= 0 && !pa.turning)
            {
                GetComponent<AudioSource>().PlayOneShot(jumpsfx, 1f);
                //vMove = jump;
                //Add Force to body to cause jump
                thisBody.velocity = new Vector2(thisBody.velocity.x, 0);
                thisBody.AddForce(jumpForce * Vector2.up, ForceMode2D.Force);
                
                jumpCooldown = jumpCooldownTime;

                // Handle jumping animation
                if (pa.facing) pa.SetAnimation("rightJump");
                else pa.SetAnimation("leftJump");
            }
            // Handle ground animation
            else if (!pa.turning && onGround())
            {

                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (pa.facing) pa.SetAnimation("rightWalk");
                    else pa.SetAnimation("leftRightTurn");
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (pa.facing) pa.SetAnimation("rightLeftTurn");
                    else pa.SetAnimation("leftWalk");
                }
                else
                {
                    if (pa.facing) pa.SetAnimation("rightIdle");
                    else pa.SetAnimation("leftIdle");
                }
            }
            // Handle free falling animation
            else if (!pa.turning && !onGround() && pa.currentAnimId != "leftJump" && pa.currentAnimId != "rightJump")
            {
                if (pa.facing) pa.SetAnimation("rightFall");
                else pa.SetAnimation("leftFall");
            }
           

            /*
            Vector2 directionMoved = new Vector2(hMove, vMove);
            thisBody.AddForce(directionMoved);
            */


        }
        else if (Input.GetAxisRaw("Horizontal") == 0 && !pa.turning && onGround())
        {
            // Update animation to idle if not moving
            if (pa.facing) pa.SetAnimation("rightIdle");
            else pa.SetAnimation("leftIdle");
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
        GameManager.instance.Respawn();
        health = 3;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (SceneManager.GetActiveScene().name == "Boss Level")
        {
            Debug.Log("changing volume");
            GetComponent<AudioSource>().volume = 1f;
        }

        if (dmg > 0)
        {
            StartCoroutine(DamageFlash());
        }
        GetComponent<AudioSource>().PlayOneShot(damagesfx);
        GetComponent<AudioSource>().volume = 0.1f;
        if (health <= 0)
        {
            Die();
        }
    }

    


    private void OnDestroy()
    {
        //print("Destroying");
        mat.friction = 0;
        //GetComponent<CapsuleCollider2D>().sharedMaterial.friction = 1;
    }

    IEnumerator DamageFlash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(.25f);
        sprite.color = Color.white;
    }




}
