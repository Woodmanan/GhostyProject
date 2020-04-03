using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 currentDirection = Vector3.right;
    public int speed = 1;

    private double spiralAngle = 0;
    private double shakeTimer = 0;
    public double shakeLength = 4;

    public double leftlimit = -2;
    public double rightlimit = 2;

    public AudioClip lasersfx;

   
    public int BossHealth = 5;
    public int DamageReceivedFromFist = 1;
    List<string> damageObjects = new List<string> {"Boulder", "Stalactites"};

    public GameObject round;
    private GameObject character;

    private double roundsPerSecond;
    private double lastShot;

    private int currentPattern;
    
    void Start()
    {
        character = GameObject.Find("Character");
        leftlimit += transform.position.x;
        rightlimit += transform.position.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        

        if (shakeTimer > 0)
        {
            shakeTimer += Time.deltaTime;
            Vector2 shakeVector = new Vector2(0, Mathf.Sin((float)shakeTimer*50)/25);
            transform.GetChild(0).transform.Translate(shakeVector);

            
            if (shakeTimer > shakeLength)
            {
                if (BossHealth <= 0)
                {
                    SendMessageUpwards("Death");
                }
                else
                {
                    transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
                    shakeTimer = 0;

                    currentPattern += 1;
                    if (currentPattern > 2)
                    {
                        currentPattern = 0;
                    }
                }
            }

        }
        else
        {
            if (currentPattern == 0)
            {
                AimPattern();
            }
            else if (currentPattern == 1)
            {
                CirclePattern();
            }
            else if (currentPattern == 2)
            {
                SpiralPattern();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "LeftHand" || collision.gameObject.name == "RightHand")
        {

            if (collision.gameObject.GetComponent<BossHand>().justPossessed)
            {
                Debug.Log("Boss has been attacked");
                BossHealth -= DamageReceivedFromFist;

                collision.gameObject.GetComponent<BossHand>().justPossessed = false;
                transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
                if (shakeTimer == 0)
                {
                    shakeTimer = .01;
                }
            }
        }

    }


    void Move()
    {

        transform.Translate(speed * currentDirection * Time.deltaTime);
        if (transform.position.x > rightlimit)
        {
            currentDirection = Vector3.left;
        }
        if (transform.position.x < leftlimit)
        {
            currentDirection = Vector3.right;
        }

    }


    private void DefaultPattern()
    {
        Vector2 currVelocity = new Vector2(0, -1);
        double currSpeed = 5;
        roundsPerSecond = 1;

        lastShot += Time.deltaTime;

       
        

        if (lastShot > 1 / roundsPerSecond)
        {
            lastShot = 0;
            GameObject newBullet = Instantiate(round, transform);
            
            newBullet.GetComponent<BossProjectile>().speed = currSpeed;
            newBullet.GetComponent<BossProjectile>().velocity = currVelocity;
            //newBullet.transform.position = transform.position;
        }

    }

    private void AimPattern()
    {
        double currSpeed = 3;
        roundsPerSecond = 1;
      

        lastShot += Time.deltaTime;

        

        if (lastShot > 1 / roundsPerSecond)
        {
            lastShot = 0;
            GetComponent<AudioSource>().PlayOneShot(lasersfx);
            GameObject newBullet = Instantiate(round);

            newBullet.GetComponent<BossProjectile>().speed = currSpeed;
            newBullet.GetComponent<BossProjectile>().lifetime = 7;
            newBullet.transform.position = transform.position;
            newBullet.GetComponent<BossProjectile>().velocity = (character.transform.position - transform.position).normalized;
        }

    }

    private void CirclePattern()
    {
        double currSpeed = 3;
        double currAngle = 0;
        double numInCircle = 7;

        Vector2 currVelocity;

        lastShot += Time.deltaTime;
        roundsPerSecond = 2;
        

        if (lastShot > 1 / roundsPerSecond)
        {
            lastShot = 0;

            for (int i = 0; i < numInCircle; i++)
            {
                currAngle = Mathf.Deg2Rad * i * 360/numInCircle;
                currVelocity = new Vector2(Mathf.Cos((float)currAngle), Mathf.Sin((float)currAngle));

                GameObject newBullet = Instantiate(round);

                newBullet.GetComponent<BossProjectile>().speed = currSpeed;
                newBullet.GetComponent<BossProjectile>().lifetime = 5;
                newBullet.transform.position = transform.position + new Vector3(currVelocity.x, currVelocity.y, 0);
                newBullet.GetComponent<BossProjectile>().velocity = currVelocity;
            }
        }

    }

    private void SpiralPattern()
    {
        double currSpeed = 5;
        
        double numInCircle = 10;
        Vector2 currVelocity;

        roundsPerSecond = 10;
        lastShot += Time.deltaTime;

      

        if (lastShot > 1 / roundsPerSecond)
        {
            lastShot = 0;

           
            spiralAngle += 360 / numInCircle;
            if (spiralAngle > 360)
            {
                spiralAngle = 0;
            }
            double currAngle = Mathf.Deg2Rad * spiralAngle;
                
            currVelocity = new Vector2(Mathf.Cos((float)currAngle), Mathf.Sin((float)currAngle));

            GameObject newBullet = Instantiate(round);

            newBullet.GetComponent<BossProjectile>().speed = currSpeed;
            newBullet.GetComponent<BossProjectile>().lifetime = 4;
            newBullet.transform.position = transform.position + new Vector3(currVelocity.x, currVelocity.y, 0);
            newBullet.GetComponent<BossProjectile>().velocity = currVelocity;
            
        }

    }
}
