using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BridgeConnector : MonoBehaviour
{
    [SerializeField] private float lengthOfBridge;
    private float length;

    private GameObject grabPiece;

    private GameObject bridgePiece;
    // Start is called before the first frame update
    void Start()
    {
        length = lengthOfBridge;
        grabPiece = transform.GetChild(1).gameObject;
        bridgePiece = grabPiece.transform.GetChild(0).gameObject;
        grabPiece.transform.localPosition = Vector3.up * length;
        bridgePiece.transform.localPosition = Vector3.down * length / 4;
        bridgePiece.GetComponent<SpriteRenderer>().size = new Vector2(.16f, length);
        bridgePiece.GetComponent<BoxCollider2D>().size = new Vector2(.16f, length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        float length = lengthOfBridge + .13f;
        Gizmos.color = new Color(150, 75, 0);
        Vector3 upDimensions = new Vector3(.1f, length, .1f);
        Vector3 sideDimensions = new Vector3(length, .1f, .1f);
        Vector3 upPosition = transform.position + new Vector3(0, length / 2, 0);
        Vector3 sidePosition = transform.position + new Vector3(length / 2, 0, 0);
        Vector3 sidePosition2 = transform.position + Vector3.left * length / 2;
        Gizmos.DrawWireCube(upPosition, upDimensions);
        Gizmos.DrawWireCube(sidePosition, sideDimensions);
        Gizmos.DrawWireCube(sidePosition2, sideDimensions);
    }
}
