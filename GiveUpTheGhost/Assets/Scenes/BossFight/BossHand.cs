using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D currentBody;
    public bool isLeftHand;
    private double speed = 300;
    double currentDirectionDegrees;
    private double leftBoundary;
    private double rightBoundary;
    private Dictionary<int, Vector2> angles;
    void Start()
    {
        currentBody = transform.gameObject.GetComponent<Rigidbody2D>();
        leftBoundary = transform.localPosition.x + transform.parent.gameObject.GetComponent<Boss>().armsRange * -1;
        rightBoundary = transform.localPosition.y + transform.parent.gameObject.GetComponent<Boss>().armsRange;

        Vector2 zero = new Vector2(Mathf.Sin(30*Mathf.Deg2Rad), Mathf.Sin(30*Mathf.Deg2Rad));
        Vector2 one = new Vector2(Mathf.Sin(45 * Mathf.Deg2Rad), Mathf.Sin(45 * Mathf.Deg2Rad));
        Vector2 two = new Vector2(Mathf.Sin(45 * Mathf.Deg2Rad), Mathf.Sin(45 * Mathf.Deg2Rad));

        currentBody.velocity = new Vector2(0, -1);
        
        angles = new Dictionary<int, Vector2>{
            {0,  zero},
            {1, one},
            {2, two }
             };
    }

    
    void FixedUpdate()
    {
        currentBody.velocity = currentBody.velocity.normalized * (float)speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name != "LeftHand")
        {
            Debug.Log("Hit the Character");
            if (Random.Range(0,3) > 1.5)
            {
                currentBody.velocity = angles[Random.Range(0, 2)]*-1;

            }
            if (Random.Range(0, 3) < 1.5)
            {
                currentBody.velocity = angles[Random.Range(0, 2)];

            }


        }
        else
        {
            Debug.Log("Not on the ground");
        }

    }
}
