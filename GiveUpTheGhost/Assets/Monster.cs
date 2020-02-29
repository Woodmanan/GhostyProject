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
    
    public float speed;
    private int accel;

    public float horizontal_margin;

    public int enemyType = 0;
    private Rigidbody2D thisBody;

    private bool isJumping = false;
    
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


}
