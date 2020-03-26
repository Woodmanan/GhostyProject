using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private Vector2[] positions;

    [SerializeField] private float speed;

    private int counter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if (positions.Length < 2)
        {
            Debug.LogError("Platform needs at least two positions");
        }
        else
        {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] += (Vector2) transform.position;
            }

            StartCoroutine(moveTo(positions[0]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator moveTo(Vector2 position)
    {
        Vector3 pos = new Vector3(position.x, position.y, transform.position.z);
        float dist = Vector3.Distance(transform.position, pos);
        float totalTime = dist / speed;
        float timer = 0;
        Vector3 startPos = transform.position;
        yield return null;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, pos, (timer/totalTime));
            yield return null;
        }

        transform.position = pos;
        
        //At our new spot, time to clean up;
        counter += 1;
        if (counter == positions.Length)
        {
            counter = 0;
        }

        StartCoroutine(moveTo(positions[counter]));
    }

    private void OnDrawGizmos()
    {
        /*if (EditorApplication.isPlaying)
        {
            return;
        }*/
        if (positions.Length < 2)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        for (int i = 0; i < positions.Length; i++)
        {
            Vector2 pos = positions[i] + (Vector2) transform.position;
            Gizmos.DrawWireSphere(new Vector3(pos.x, pos.y, 0), .2f);
        }

        for (int i = 0; i < positions.Length - 1; i++)
        {
            Vector3 pos1 = (Vector3) positions[i] + transform.position;
            Vector3 pos2 = (Vector3) positions[i + 1] + transform.position;
            Gizmos.DrawLine(pos1, pos2);
        }

        Gizmos.DrawLine((Vector3) positions[positions.Length - 1] + transform.position,
            (Vector3) positions[0] + transform.position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Body"))
        {
            Rigidbody2D rig = other.gameObject.GetComponent<Rigidbody2D>();
            other.transform.SetParent(transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Body"))
        {
            other.transform.SetParent(null);
        }
    }
}
