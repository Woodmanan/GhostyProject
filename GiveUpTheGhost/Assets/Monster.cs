using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // Start is called before the first frame update
    private List<string> floors = new List<string>() { "Floor", "Platform" };


    private float left_limit;
    private float right_limit;
    private int curr_direction = 0;

    public float boundary;
    public float jumpForce;
    public bool inAir;
    public bool hitJumpSignal = false;

    [SerializeField] private int health;
    
    
    public float speed;
    private int accel;

    public float horizontal_margin;

    public int enemyType = 0;
    private Rigidbody2D thisBody;

    private bool isJumping = false;

    private GameObject mainCharacterFound;
    private bool characterFound = false;
    
    void Start()
    {

    }
    
    void Awake()
    {
        thisBody = GetComponent<Rigidbody2D>();
        thisBody.freezeRotation = true;

        left_limit = transform.localPosition.x - boundary;
        right_limit = transform.localPosition.x + boundary;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 0 is right, 1 is false
        if (transform.localPosition.x < left_limit + horizontal_margin)
        {
            if (curr_direction == 1)
            {
                curr_direction = 0;
            }
        }

        if (transform.localPosition.x > right_limit - horizontal_margin)
        {
            if (curr_direction == 0)
            {
                curr_direction = 1;
            }
        }


        if (characterFound)
        {
            if (transform.localPosition.x < mainCharacterFound.transform.localPosition.x)
            {
                curr_direction = 0;

            }
            else if (transform.localPosition.x > mainCharacterFound.transform.localPosition.x)
            {
                curr_direction = 1;
            }

            
            /*if (mainCharacterFound.transform.localPosition.x < left_limit + horizontal_margin)
            {
                Debug.Log("CharacterFound False");
                characterFound = false;

            }

            if (mainCharacterFound.transform.localPosition.x > right_limit - horizontal_margin)
            {
                characterFound = false;
                Debug.Log("CharacterFound False");

            }*/
            

        }





        if(curr_direction == 0)
        {

            SpriteRenderer currentImage = transform.Find("MonsterSprite").GetComponent<SpriteRenderer>();
            currentImage.flipX = false;

        }
        else
        {
            SpriteRenderer currentImage = transform.Find("MonsterSprite").GetComponent<SpriteRenderer>();
            currentImage.flipX = true;

        }

        if (enemyType == 0)
        {
            enemyTypeZero();
        }
        
        
        

    }


    void enemyTypeZero()
    {

        Vector3 newPosition = transform.localPosition;

        if (curr_direction == 0)
        {

            newPosition = speed * Vector3.right;

        }
        else if (curr_direction == 1)
        {

            newPosition = speed * Vector3.left;

        }

        if (isJumping)
        {
            Debug.Log("jumpforce");
            thisBody.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            isJumping = false;
            inAir = true;


        }

        transform.Translate(newPosition);
        

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "AreaSignal")
        {

            AreaSignal signalSpace = collider.gameObject.GetComponent<AreaSignal>();

            if (((signalSpace.jump == true) && (isJumping == false)) && inAir == false)
            {
                isJumping = true;
                hitJumpSignal = true;



            }

            if (signalSpace.reverse == true)
            {
                
            }

        }



    }

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (inAir)
        {

            if (floors.Contains(collision.gameObject.name))
            {
                inAir = false;
            }


        }


    }

    public void SetMainCharacterPosition(GameObject character)
    {

        mainCharacterFound = character;

        characterFound = true;
        Debug.Log("FOUND THE CHARACTER");

    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            print("Monster has died!");
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 rightBound = transform.right * boundary;
        Vector3 right = transform.position + rightBound;
        Vector3 left = transform.position - rightBound;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(right, Vector3.one);
        Gizmos.DrawWireCube(left, Vector3.one);
        Gizmos.DrawLine(left, right);
    }
}
