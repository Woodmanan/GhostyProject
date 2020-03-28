using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    // Start is called before the first frame update
    private bool possessed = false;
    Rigidbody2D currentBody;
    public bool isLeftHand;
    private double speed = 300;
    double currentDirectionDegrees;
    private double leftBoundary;
    private double rightBoundary;
    private Dictionary<int, Vector2> angles;

    private Transform ghost;
    private Vector2 lastVelocity;
    private bool checkForPress = false;
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

    
    void FixedUpdate()
    {
        if (possessed == false)
        {
            currentBody.velocity = currentBody.velocity.normalized * (float)speed * Time.deltaTime;
        }
        
        if (possessed)
        {
            Debug.Log("Possessed");
            transform.position = ghost.position;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                possessed = false;
                currentBody.velocity = lastVelocity;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (possessed == false)
        {
            if (collision.collider.name != "LeftHand" || collision.collider.name != "Character")
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

    }

   
    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.name == "Ghost")
        {

            if (trigger.gameObject.GetComponent<Ghost>().ghostMode == true)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    lastVelocity = currentBody.velocity;
                    currentBody.velocity = new Vector2(0, 0);
                    possessed = true;
                    ghost = trigger.transform;

                }

            }
        }
    }
}
