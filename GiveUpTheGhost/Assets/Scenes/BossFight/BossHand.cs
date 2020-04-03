using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    // Start is called before the first frame update

    public int damage = 2;

    public bool possessed = false;
    public bool justPossessed = false;
    Rigidbody2D currentBody;

    public AudioClip handssfx;

    private double speed = 300;
    private Dictionary<int, Vector2> angles;
    public double handAttackSpeed = 3;
    private double possessionDelay = 2;
    private double possessionTimer = 0;
    private Transform ghost;
    private bool GhostInside = false;
    private Vector2 lastVelocity;
    private bool checkForPress = false;
    public AreaSignal Left;
    public AreaSignal Right;

    private int numPresses = 0;
    void Start()
    {
        currentBody = transform.gameObject.GetComponent<Rigidbody2D>();

        Vector2 zero = new Vector2(Mathf.Cos(30 * Mathf.Deg2Rad), Mathf.Sin(30 * Mathf.Deg2Rad));
        Vector2 one = new Vector2(Mathf.Cos(45 * Mathf.Deg2Rad), Mathf.Sin(45 * Mathf.Deg2Rad));
        Vector2 two = new Vector2(Mathf.Cos(60 * Mathf.Deg2Rad), Mathf.Sin(60 * Mathf.Deg2Rad));
        Vector2 three = new Vector2(Mathf.Cos(120 * Mathf.Deg2Rad), Mathf.Sin(120 * Mathf.Deg2Rad));
        Vector2 four = new Vector2(Mathf.Cos(135 * Mathf.Deg2Rad), Mathf.Sin(135 * Mathf.Deg2Rad));
        Vector2 five = new Vector2(Mathf.Cos(150 * Mathf.Deg2Rad), Mathf.Sin(150 * Mathf.Deg2Rad));
        Vector2 right = new Vector2(Mathf.Cos(0 * Mathf.Deg2Rad), Mathf.Sin(0 * Mathf.Deg2Rad));
        Vector2 left = new Vector2(Mathf.Cos(180 * Mathf.Deg2Rad), Mathf.Sin(180 * Mathf.Deg2Rad));
        Vector2 up = new Vector2(Mathf.Cos(90 * Mathf.Deg2Rad), Mathf.Sin(90 * Mathf.Deg2Rad));
        Vector2 down = new Vector2(Mathf.Cos(270 * Mathf.Deg2Rad), Mathf.Sin(270 * Mathf.Deg2Rad));

        currentBody.velocity = new Vector2(Random.Range(-1, 1), -1);

        angles = new Dictionary<int, Vector2>{
            {0,  zero},
            {1, one},
            {2, two },
            {3,  three},
            {4, four},
            {5, five},
            {6, right},
            {7, left},
            {8, up},
            {9, down}
             };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (possessed == true)
            {
                possessed = false;
                justPossessed = true;
                possessionTimer = .01;
               
                Vector2 towardsHead = transform.localPosition.normalized;
                currentBody.velocity = -(float)handAttackSpeed * towardsHead;

            }

            if (GhostInside == true)
            {
                numPresses += 1;
               
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            if (possessed == true)
            {
                possessed = false;
                justPossessed = true;
                possessionTimer = .01;

                Vector2 towardsHead = transform.localPosition.normalized;
                currentBody.velocity = -(float)handAttackSpeed * towardsHead;


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
            GetComponent<AudioSource>().PlayOneShot(handssfx);
            if (collision.collider.name != "Character")
            {

                if (Random.Range(0, 4) > 1.5)
                {
                    currentBody.velocity = angles[Random.Range(0, 5)] * -1;

                }
                else
                {
                    currentBody.velocity = angles[Random.Range(0, 5)];

                }

                if (collision.collider.name != "BossHead")
                {
                    justPossessed = false;
                }


            }
            else
            {
                
            }
        }

        if (collision.collider.name == "Character")
        {
            if (collision.collider.transform.position.y - currentBody.transform.position.y > 0)
            {
                currentBody.velocity = new Vector2(0, 0);

            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Character")
        {
            if (collision.transform.position.y - currentBody.transform.position.y > 0)
            {
                currentBody.velocity = new Vector2(0, 1);
            }
            if (!possessed)
            {
                if (transform.position.y - collision.transform.position.y > 0)
                {
                    collision.gameObject.GetComponent<Character>().TakeDamage(damage/2);

                }
                else
                {
                    collision.gameObject.GetComponent<Character>().TakeDamage(damage);


                }
            }
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

    private void MovementPatterns()
    {

    }
}
