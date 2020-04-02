using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Possessable poss;

    private GameObject body;
    // Start is called before the first frame update
    void Start()
    {
        poss = GetComponent<Possessable>();
        body = GameObject.FindGameObjectWithTag("Body");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (poss.possessed)
        {
            Orient();
        }
    }

    private void Orient()
    {
        Vector3 offset = transform.position - body.transform.position;
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0, angle + 90);
    }
}
