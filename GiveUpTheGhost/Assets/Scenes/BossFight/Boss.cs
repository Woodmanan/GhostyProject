using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isAlive = true;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }

    void WeakSpotReachedZero(int weakspotID)
    {
        if (weakspotID == 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }

    }

    

    
}
