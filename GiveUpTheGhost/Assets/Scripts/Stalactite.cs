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

    public AudioClip sfx;
    
    private Character body;
    private Ghost ghost;
    private Possessable possess;
    private DistanceJoint2D joint;
    private bool disconnected;
    private bool active;

    private bool falling;

    private Vector3 whereToSpawn;
    private Quaternion respawnRotation;

    [SerializeField] private bool stayAfterFalling;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        body = GameObject.FindGameObjectWithTag("Body").GetComponent<Character>();
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        possess = GetComponent<Possessable>();

        whereToSpawn = transform.position;
        respawnRotation = transform.rotation;
        
        
        falling = false;

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
            if (Input.GetKeyDown(KeyCode.Z) && !disconnected)
            {
                stopPossession();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && !disconnected)
            {
                stopPossession();
                ghost.disableGhostMode();
            }

            //See if we can pull
            if (!disconnected && Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (timer < 0)
                {
                    timer = delay;
                    tugs -= 1;
                    GetComponent<AudioSource>().PlayOneShot(sfx);
                    if (tugs == 0)
                    {
                        //Turn off this piece!
                        possess.enabled = true;
                        disconnected = true;
                        rig.simulated = true;
                        stopPossession();
                        if (stayAfterFalling)
                        {
                            GetComponent<Stalactite>().enabled = false;
                        }
                        else
                        {
                            //possess.enabled = false;
                            GetComponent<PossesTexControls>().reset();
                            GetComponent<PossesTexControls>().enabled = false;
                            StartCoroutine(setFallingAfterTimer(.2f));
                        }
                        
                        
                    }

                    GetComponent<ParticleSystem>().Emit(100 - (40 * tugs));
                }
            }
        }
    }

    public void possessionStarts()
    {
        if (disconnected) return;
        possess.enabled = false;
        StartCoroutine(setActiveAfterTimer(.1f));
    }

    IEnumerator setActiveAfterTimer(float time)
    {
        yield return new WaitForSeconds(time);
        active = true;
    }
    
    IEnumerator setFallingAfterTimer(float time)
    {
        yield return new WaitForSeconds(time);
        falling = true;
    }
    

    public void stopPossession()
    {
        possess.enabled = true;
        possess.stopPossession();
        active = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (falling)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<ParticleSystem>().Emit(100);
            falling = false;
            StartCoroutine(respawn(.2f));
        }
    }

    IEnumerator respawn(float timer)
    {
        print("We're respawning!");
        yield return new WaitForSeconds(timer);
        transform.position = whereToSpawn;
        transform.rotation = respawnRotation;

        print("Respawned!");

        possess.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<PossesTexControls>().enabled = true;

        Start();
        
        tugs = 3;
    }
}
