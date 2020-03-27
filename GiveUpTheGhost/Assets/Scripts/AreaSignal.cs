using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSignal : MonoBehaviour
{
    // Objects react to this object, this object does not react, it only stores information
    public int state_0 = 0;
    public int state_1 = 0;
    public int state_2 = 0;
    public int state_3 = 0;
    public bool jump = false;
    public bool reverse = false;
    public bool teleport = false;
    public AreaSignal teleport_to;

    void Awake()
    {
        SpriteRenderer symbol = transform.Find("Symbol").GetComponent<SpriteRenderer>();
        symbol.color = new Color32(0, 0, 0, 0);
    }
    
    void Start()
    {


    }

}
