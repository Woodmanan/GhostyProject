using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDamage : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Body"))
        {
            other.gameObject.GetComponent<Character>().TakeDamage(damage);
        }
        //TODO: Fill this in for monsters
        print("Collided with something!");
        StartCoroutine(DestroyThis(2f));
    }

    IEnumerator DestroyThis(float delay)
    {
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
