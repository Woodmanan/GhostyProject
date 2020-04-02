using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnTouch : MonoBehaviour
{
    [SerializeField] private int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Is it the player?
        if (other.gameObject.CompareTag("Body"))
        {
            //It is! Dealing damage
            print("Dealing " + damage + " damage to the player!");
            other.gameObject.GetComponent<Character>().TakeDamage(damage);
        }
    }
}
