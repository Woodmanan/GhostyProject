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
        Destroy(this.gameObject);
        Destroy(this);

        

    }
}
