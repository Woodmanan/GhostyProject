using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    // Start is called before the first frame update
    private bool possessed = false;
    Rigidbody2D currentBody;
    public bool isLeftHand;
    private double speed = 1;
    private double maxspeed = 300;
    double currentDirectionDegrees;
    private double leftBoundary;
    private double rightBoundary;
    private Dictionary<int, Vector2> angles;

    private Transform ghost;
    private Vector2 lastVelocity;
    private Vector2 currVelocity;
    private bool checkForPress = false;
    private double possessionTime = 0;
    private double possessionLimit = 5;
    private double lastPossession = 100;

    private bool isUnderCharacter = false;
    private PhysicsMaterial2D currentMaterial;
    void Start()
    {
        currentBody = transform.gameObject.GetComponent<Rigidbody2D>();
        currentMaterial = currentBody.sharedMaterial;


        Vector2 zero = new Vector2(Mathf.Sin(30*Mathf.Deg2Rad), Mathf.Sin(30*Mathf.Deg2Rad));
        Vector2 one = new Vector2(Mathf.Sin(45 * Mathf.Deg2Rad), Mathf.Sin(45 * Mathf.Deg2Rad));
        Vector2 two = new Vector2(Mathf.Sin(60 * Mathf.Deg2Rad), Mathf.Sin(60 * Mathf.Deg2Rad));
        Vector2 three = new Vector2(Mathf.Sin(120 * Mathf.Deg2Rad), Mathf.Sin(120 * Mathf.Deg2Rad));
        Vector2 four = new Vector2(Mathf.Sin(135 * Mathf.Deg2Rad), Mathf.Sin(135 * Mathf.Deg2Rad));
        Vector2 five = new Vector2(Mathf.Sin(150 * Mathf.Deg2Rad), Mathf.Sin(150 * Mathf.Deg2Rad));

        currVelocity = new Vector2(0, -1);
        
        angles = new Dictionary<int, Vector2>{
            {0,  zero},
            {1, one},
            {2, two },
            {3,  three},
            {4, four},
            {5, five}
             };
    }

    
    void FixedUpdate()
    {
        if (possessed == false)
        {

            currentBody.transform.Translate(currVelocity * (float)speed * Time.deltaTime);
        }
        
        if (possessed)
        {
            transform.position = ghost.position;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                possessed = false;
                

            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (possessed == false)
        {
            
            
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (possessed == false)
        {
            if (collision.collider.name == "Character")
            {

                speed = maxspeed;
            }
        }
    }
   
    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.name == "Ghost")
        {
            if (possessed == false)
            {
                if (trigger.gameObject.GetComponent<Ghost>().ghostMode == true)
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        speed = 0;
                        possessed = true;
                        ghost = trigger.transform;

                        
                    }

                }
            }
        }
    }

    
}
