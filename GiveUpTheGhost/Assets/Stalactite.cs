using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Possessable))]
public class Stalactite : MonoBehaviour
{
    [SerializeField] private int tugs;

    private Rigidbody2D rig;

    [SerializeField] private float delay;
    private float timer;
    
    private Character body;
    private Ghost ghost;
    private Possessable possess;
    private DistanceJoint2D joint;
    private bool disconnected;
    private bool active;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        body = GameObject.FindGameObjectWithTag("Body").GetComponent<Character>();
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        possess = GetComponent<Possessable>();

        rig.simulated = false;

        //Set up our joint
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        disconnected = false;
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (active)
        {
            //See if we disconnect
            if (Input.GetKeyDown(KeyCode.LeftShift) && !disconnected)
            {
                stopPossession();
            }

            //See if we can pull
            if (!disconnected && Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (timer < 0)
                {
                    timer = delay;
                    tugs -= 1;
                    if (tugs == 0)
                    {
                        //Turn off this piece!
                        possess.enabled = true;
                        disconnected = true;
                        rig.simulated = true;
                        GetComponent<Stalactite>().enabled = false;
                    }

                    GetComponent<ParticleSystem>().Emit(100);
                }
            }
        }
    }

    public void possessionStarts()
    {
        if (disconnected) return;
        possess.enabled = false;
        active = true;
    }
    
    public void stopPossession()
    {
        possess.enabled = true;
        possess.stopPossession();
        active = false;
    }
    
}
