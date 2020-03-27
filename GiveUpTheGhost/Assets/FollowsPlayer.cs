using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FollowsPlayer : MonoBehaviour
{
    private GameObject body;
    [SerializeField] private float startX;
    [SerializeField] private float endX;
    private Vector2 yz;
    [SerializeField] private float lerpAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GameObject.FindGameObjectWithTag("Body");
        yz = new Vector2(transform.position.y, transform.position.z);
        if (startX > endX)
        {
            print("Camera: Start and end are flipped");
            float hold = startX;
            startX = endX;
            endX = hold;
        }
    }

    // This should be in LateUpdate
    //But when it is it doesn't work right
    //I'm not sure why, oh god I'm sorry
    void FixedUpdate()
    {
        float goalX = body.transform.position.x;
        if (goalX < startX)
        {
            goalX = startX;
        }

        if (goalX > endX)
        {
            goalX = endX;
        }
        
        Vector3 goal = new Vector3(goalX, yz.x, yz.y);
        
        transform.position = Vector3.Lerp(transform.position, goal, lerpAmount);
    }

    private void OnDrawGizmos()
    {
        Vector3 center = new Vector3((startX + endX) / 2, transform.position.y, 0);
        Vector3 dimensions = new Vector3(Mathf.Abs(startX - endX), 10, 1);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(center, dimensions);
    }
}
