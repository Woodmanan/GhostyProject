using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class Possessable : MonoBehaviour
{
    [SerializeField] private String horizontalAxis;

    [SerializeField] private String verticalAxis;

    public bool possessed;
    [SerializeField] private float distToPossess;
    [SerializeField] private float speed = 1;

    //Collision variables
    [SerializeField] private float speedToDamage;
    [SerializeField] private int damage;
    private float actualSpeed;
    
    private Vector2 target;
    private Rigidbody2D rig;

    private Character body;
    private Ghost ghost;
    private DistanceJoint2D joint;

    public AudioClip possessSFX;
    public AudioClip unpossessSFX;
    private AudioSource soundSource;

    [SerializeField] private bool enableGravityOnRelease = true;
    [SerializeField] private bool stopVelocityOnRelease = false;

    //Event for possession start
    public UnityEvent possessionBegins;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody2D>();
        body = GameObject.FindGameObjectWithTag("Body").GetComponent<Character>();
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        possessed = false;
        
        //Set up our joint
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        actualSpeed = 0;
    }

    private void FixedUpdate()
    {
        joint.connectedAnchor = body.gameObject.transform.position;
        actualSpeed = rig.velocity.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (possessed)
        {
            Vector2 offset = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
            if (offset.magnitude > 1)
            {
                offset = offset.normalized;
            }
            //target += offset;
            rig.velocity = offset * speed;
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                soundSource.PlayOneShot(unpossessSFX);
                stopPossession();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && ghost.ghostMode)
            {
                if (Vector2.Distance(transform.position, ghost.transform.position) < distToPossess)
                {
                    //Do the expensive check now
                    if (body.GetBeneath() == this.gameObject)
                    {
                        print("Didn't possess!");
                        StartCoroutine(FlashRed(1.0f, .25f));
                    }
                    else
                    {
                        soundSource.PlayOneShot(possessSFX);
                        Possess();
                    }
                }
                
            }
        }
    }

    IEnumerator FlashRed(float timeToFlash, float timeToHold)
    {
        yield return null;
        float timer = 0;
        bool red = false;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        while (timer < timeToFlash)
        {
            timer += timeToHold;
            if (red)
            {
                sprite.color = Color.white;
            }
            else
            {
                sprite.color = Color.red;
            }

            red = !red;
            yield return new WaitForSeconds(timeToHold);
        }

        sprite.color = Color.white;
    }

    IEnumerator startPossession(float lerpTime, Vector2 offset)
    {
        rig.gravityScale = 0;
        Vector2 velocityStart = rig.velocity;
        for (float i = 0; i < lerpTime; i += Time.deltaTime)
        {
            rig.velocity = Vector2.Lerp(velocityStart, Vector2.zero, i/lerpTime);
            yield return null;
        }

        rig.velocity = Vector2.zero;
        this.possessed = true;

        target = rig.position + offset;
        possessionBegins.Invoke();
    }
    

    public void Possess()
    {
        ghost.gameObject.SetActive(false);
        joint.enabled = true;
        joint.connectedAnchor = body.getPosition();
        joint.distance = body.getDistance();
        if (rig.velocity.magnitude < 1)
        {
            StartCoroutine(startPossession(0f, new Vector2(0, 0.2f)));
            
        }
        else
        {
            StartCoroutine(startPossession(.5f, new Vector2(0, 0.3f)));
        }


    }

    public void stopPossession()
    {
        ghost.gameObject.SetActive(true);
        ghost.GetComponent<Rigidbody2D>().position = rig.position;
        this.possessed = false;
        joint.enabled = false;
        if (enableGravityOnRelease)
        {
            rig.gravityScale = 1;
        }

        if (stopVelocityOnRelease)
        {
            rig.velocity = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distToPossess);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distToPossess * (actualSpeed / speedToDamage));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (actualSpeed >= speedToDamage)
        {
            //Player Damage
            if (other.gameObject.CompareTag("Body"))
            {
                other.gameObject.GetComponent<Character>().TakeDamage(damage);
            }
            
            //Monster dmg
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Monster>().TakeDamage(damage);
            }
        }
    }
}
