using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Possessable : MonoBehaviour
{
    [SerializeField] private String horizontalAxis;

    [SerializeField] private String verticalAxis;

    public bool possessed;
    [SerializeField] private float distToPossess;
    [SerializeField] private float speed = 1;

    private Vector2 target;
    private Rigidbody2D rig;

    private Character body;
    private Ghost ghost;
    private DistanceJoint2D joint;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        body = GameObject.FindGameObjectWithTag("Body").GetComponent<Character>();
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        possessed = false;
        
        //Set up our joint
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    private void FixedUpdate()
    {
        joint.connectedAnchor = body.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (possessed)
        {
            Vector2 offset = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
            //target += offset;
            rig.velocity = offset * speed;
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                
                stopPossession();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && ghost.ghostMode)
            {
                print("Checking!");
                print("Distance: " + Vector2.Distance(transform.position, ghost.transform.position));
                if (Vector2.Distance(transform.position, ghost.transform.position) < distToPossess)
                {
                    
                    Possess();
                }
                
            }
        }
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
        rig.gravityScale = 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distToPossess);
    }
}
