using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 currentDirection = Vector3.right;
    private int speed = 5;

    private int leftlimit = -2;
    private int rightlimit = 2;

    public int weakspotID;
    public int weakSpotHealth;
    public int damageValue;
    private bool weakSpotActive = true;
    List<string> damageObjects = new List<string> {"Boulder", "Stalactites"};
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    void OnTriggerEnter(Collider other)
    {
        if (weakSpotActive == true)
        {
            if (damageObjects.Contains(other.name))
            {

                Debug.Log("Damage");
                if (weakSpotHealth - damageValue > 0)
                {
                    weakSpotHealth = weakSpotHealth - damageValue;
                    
                }
                else
                {
                    SendMessageUpwards("WeakSpotReachedZero", weakspotID);
                    weakSpotActive = false;
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
}
