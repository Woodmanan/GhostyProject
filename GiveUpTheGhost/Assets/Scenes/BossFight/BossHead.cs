using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 currentDirection = Vector3.right;
    private int speed = 1;

    private double shakeTimer = 0;
    private double shakeLength = 2;

    private int leftlimit = -2;
    private int rightlimit = 2;

   
    public int BossHealth = 5;
    public int DamageReceivedFromFist = 1;
    List<string> damageObjects = new List<string> {"Boulder", "Stalactites"};

    public GameObject round;
    private GameObject character;

    private double roundsPerSecond;
    private double lastShot;

    
    void Start()
    {
        character = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        DefaultPattern();

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
                }
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
        
        lastShot += Time.deltaTime;

        if (roundsPerSecond != 1)
        {
            roundsPerSecond = 1;
        }

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
        Vector2 currVelocity = new Vector2(0, -1);
        double currSpeed = 3;

        lastShot += Time.deltaTime;

        if (roundsPerSecond != 5)
        {
            roundsPerSecond = 1;
        }

        if (lastShot > 1 / roundsPerSecond)
        {
            lastShot = 0;
            GameObject newBullet = Instantiate(round);

            newBullet.GetComponent<BossProjectile>().speed = currSpeed;
            newBullet.GetComponent<BossProjectile>().lifetime = 10;
            newBullet.transform.position = transform.position;
            newBullet.GetComponent<BossProjectile>().velocity = (character.transform.position - transform.position).normalized;
        }

    }
}
