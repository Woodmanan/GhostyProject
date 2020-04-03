using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ghostMode = false;
    [SerializeField] private float speed = .4f;
    private DistanceJoint2D joint;
    private Character body;
    private Rigidbody2D rigid;
    private CircleController radius;
    private SpriteRenderer sprite;

    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        body = GameObject.FindGameObjectWithTag("Body").GetComponent<Character>();
        //joint.enabled = false;
        joint.distance = 0;//body.getDistance();
        joint.enabled = false;
        rigid = GetComponent<Rigidbody2D>();
        rigid.simulated = false;
        radius = transform.parent.GetComponentInChildren<CircleController>();
        radius.setRad(body.getDistance());
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    //Events that need to happen before physics
    void FixedUpdate()
    {
        //Set the updated anchor position
        joint.connectedAnchor = body.gameObject.transform.position;
    }
    
    // Update is called once per frame
    private void Update()
    {
        GhostMovement();
    }

    IEnumerator setGhost(float timer)
    {
        float dist = body.getDistance();
        for (float i = timer; i >= 0; i -= Time.deltaTime)
        {
            //Fancy lerp sliding
            joint.distance = Mathf.Lerp(0, dist, i / timer);
            radius.setLerp(1 - i/timer);
            yield return null;
        }
        radius.setLerp(1);
        joint.distance = dist;
        body.setFriction(.5f);
    }

    public void enableGhostMode()
    {
        ghostMode = true;
        rigid.simulated = true;
        joint.enabled = true;
        sprite.enabled = true;
        StartCoroutine(setGhost(.5f));
    }

    IEnumerator returnGhost(float timer)
    {
        float dist = joint.distance;
        for (float i = 0; i < timer; i += Time.deltaTime)
        {
            //Fancy lerp sliding
            joint.distance = Mathf.Lerp(dist, 0, i / timer);
            radius.setLerp(1 - i/timer);
            yield return null;
        }
        radius.setLerp(0);
        joint.distance = 0;
        joint.enabled = false;
        body.ghostMode = false;
        rigid.simulated = false;
        sprite.enabled = false;
        body.setFriction(0);
    }

    public void disableGhostMode()
    {
        ghostMode = false;
        StartCoroutine(returnGhost(.5f));
    }


    private void GhostMovement()
    {
        if (ghostMode)
        {
            Vector2 vel = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (vel.magnitude > 1)
            {
                vel = vel.normalized;
            }
            rigid.velocity = vel * speed;
            
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {

                float hMove = Input.GetAxisRaw("Horizontal") * speed;
                float vMove = Input.GetAxisRaw("Vertical") * speed;

                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    sprite.flipX = false;
                    
                }
                else if(Input.GetAxisRaw("Horizontal") < 0)
                {
                    sprite.flipX = true;

                }

                /*
                Vector3 directionMoved = new Vector3(hMove, vMove, 0);
                Vector3 oppositeMove = new Vector3(0,0,0);
       
                
                if (transform.localPosition.x < 0)
                {
                    if (transform.localPosition.y > 0)
                    {//Quadrant II
                        Debug.Log("Quadrant II");
                        if (Input.GetAxisRaw("Horizontal") > 0)
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                        else
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                        }
                    }
                    else
                    {   //Quadrant III
                        Debug.Log("Quadrant III");
                        if (Input.GetAxisRaw("Horizontal") >= 0)
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {

                            }
                            else
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                        }
                        else
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                            else if(Input.GetAxisRaw("Vertical") == 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                        }
                    }
                }
                else
                {
                    if (transform.localPosition.y > 0)
                    {//Quadrant I
                        Debug.Log("Quadrant I");
                        if (Input.GetAxisRaw("Horizontal") > 0)
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else if(Input.GetAxisRaw("Vertical") == 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                        else
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                    }
                    else
                    {//Quadrant IV
                        Debug.Log("Quadrant IV");
                        if (Input.GetAxisRaw("Horizontal") > 0)
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else if (Input.GetAxisRaw("Vertical") == 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                        else
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                    }
                }
                
               
                

                if ((transform.localPosition + directionMoved).magnitude < radius)
                {
                    transform.Translate(directionMoved);


                }
                else
                {

                    transform.Translate(oppositeMove/13);

                }
                */




            }

        }

    }

}
