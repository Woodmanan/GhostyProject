using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    // Start is called before the first frame update
    private bool possessed = false;
    Rigidbody2D currentBody;

    private double speed = 300;
    double currentDirectionDegrees;
    private double leftBoundary;
    private double rightBoundary;
    private Dictionary<int, Vector2> angles;

    private double possessionDelay = 2;
    private double possessionTimer = 0;
    private Transform ghost;
    private bool GhostInside = false;
    private Vector2 lastVelocity;
    private bool checkForPress = false;

    private int numPresses = 0;
    void Start()
    {
        currentBody = transform.gameObject.GetComponent<Rigidbody2D>();
        leftBoundary = transform.localPosition.x + transform.parent.gameObject.GetComponent<Boss>().armsRange * -1;
        rightBoundary = transform.localPosition.y + transform.parent.gameObject.GetComponent<Boss>().armsRange;

        Vector2 zero = new Vector2(Mathf.Sin(30*Mathf.Deg2Rad), Mathf.Sin(30*Mathf.Deg2Rad));
        Vector2 one = new Vector2(Mathf.Sin(45 * Mathf.Deg2Rad), Mathf.Sin(45 * Mathf.Deg2Rad));
        Vector2 two = new Vector2(Mathf.Sin(60 * Mathf.Deg2Rad), Mathf.Sin(60 * Mathf.Deg2Rad));
        Vector2 three = new Vector2(Mathf.Sin(120 * Mathf.Deg2Rad), Mathf.Sin(120 * Mathf.Deg2Rad));
        Vector2 four = new Vector2(Mathf.Sin(135 * Mathf.Deg2Rad), Mathf.Sin(135 * Mathf.Deg2Rad));
        Vector2 five = new Vector2(Mathf.Sin(150 * Mathf.Deg2Rad), Mathf.Sin(150 * Mathf.Deg2Rad));

        currentBody.velocity = new Vector2(0, -1);
        
        angles = new Dictionary<int, Vector2>{
            {0,  zero},
            {1, one},
            {2, two },
            {3,  three},
            {4, four},
            {5, five}
             };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (possessed == true)
            {
                possessed = false;
                possessionTimer = .01;
                currentBody.velocity = new Vector2(0,-1);
                
            }

            if (GhostInside == true)
            {
                numPresses += 1;
                Debug.Log("Pressed On Time" + numPresses.ToString());
                if (ghost.gameObject.GetComponent<Ghost>().ghostMode == true)
                {
                    if (possessionTimer == 0)
                    {

                        lastVelocity = currentBody.velocity;
                        currentBody.velocity = new Vector2(0, 0);
                        possessed = true;



                    }

                }
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (possessed == true)
            {
                possessed = false;
                possessionTimer = .01;
                currentBody.velocity = new Vector2(0,-1);

            }
        }
    }
    void FixedUpdate()
    {
       
        if (possessed)
        {
            transform.position = ghost.position;
            
        }
        else
        {
            
            currentBody.velocity = currentBody.velocity.normalized * (float)speed * Time.deltaTime;
        }

        if (possessionTimer >= .01)
        {
            possessionTimer += Time.deltaTime;
            if (possessionTimer > possessionDelay)
            {
                possessionTimer = 0;
            }
        }

        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (possessed == false)
        {
            if (collision.collider.name != "Character")
            {
                if (Random.Range(0, 3) > 1.5)
                {
                    currentBody.velocity = angles[Random.Range(0, 5)] * -1;

                }
                if (Random.Range(0, 3) < 1.5)
                {
                    currentBody.velocity = angles[Random.Range(0, 5)];

                }


            }
            else
            {
                Debug.Log("Not on the ground");
            }
        }

        if (collision.collider.name == "Character")
        {
            if (collision.collider.transform.position.y - currentBody.transform.position.y > 0)
            {
                Debug.Log("CharacterIsHere");
                currentBody.velocity = new Vector2(0, 0);

            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Character")
        {

            currentBody.velocity = new Vector2(0, 1);
        }
    }




    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.name == "Ghost")
        {
            ghost = trigger.transform;
            GhostInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {

        if (trigger.name == "Ghost")
        {
            GhostInside = false;
        }

    }
}
