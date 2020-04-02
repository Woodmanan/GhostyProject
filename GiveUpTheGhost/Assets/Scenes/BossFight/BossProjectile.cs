using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 velocity;
    public double speed;
    public double lifetime = 4;
    private double timer;
    public int damage = 1;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        transform.Translate(velocity * (float)speed * Time.deltaTime);

        if (timer > lifetime)
        {
            Destroy(this.gameObject);
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.name == "Character")
        {
            trigger.GetComponent<Character>().TakeDamage(damage);
        }

        if (trigger.name != "BossProjectile")
        {
            Destroy(this.gameObject);
            Destroy(transform);
        }

        

    }
}
