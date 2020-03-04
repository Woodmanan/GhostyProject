using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    [SerializeField] private int tugs;

    private Rigidbody2D rig;

    [SerializeField] private float delay;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (rig.velocity.magnitude > 0)
        {
            rig.velocity = Vector2.zero;
            if (timer < 0)
            {
                timer = delay;
                tugs -= 1;
                if (tugs == 0)
                {
                    GetComponent<Stalactite>().enabled = false;
                }
            }
        }
    }
}
