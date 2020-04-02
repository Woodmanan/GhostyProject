using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    private Material mat;
    [SerializeField] private float radius;

    [SerializeField] private float thiccnes;
    [SerializeField] private float lerpAmount;
    [SerializeField] private float offsetSpeed;
    private float offset;

    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;
    private float distanceOffset;
    [SerializeField] private float sizeSpeed;
    
    private static readonly int RadOne = Shader.PropertyToID("_RadOne");
    private static readonly int RadTwo = Shader.PropertyToID("_RadTwo");
    private static readonly int Pos = Shader.PropertyToID("_Pos");
    private static readonly int Offset = Shader.PropertyToID("_Offset");
    private static readonly int OffMult = Shader.PropertyToID("_OffMult");

    // Start is called before the first frame update
    void Start()
    {
        offset = 0;
        mat = GetComponent<MeshRenderer>().material;
        lerpAmount = 0;
        distanceOffset = minDist;
    }

    // Update is called once per frame
    void Update()
    {
        offset += offsetSpeed * Time.deltaTime;
        float innerRad = radius;
        float outerRad = radius + thiccnes;

        distanceOffset = Mathf.PingPong(Time.time * sizeSpeed, maxDist - minDist) + minDist;
        
        mat.SetFloat(RadOne, outerRad * lerpAmount);
        mat.SetFloat(RadTwo, innerRad * lerpAmount);
        mat.SetVector(Pos, transform.position);
        mat.SetFloat(Offset, offset);
        mat.SetFloat(OffMult, distanceOffset);
    }

    public void setLerp(float lerp)
    {
        lerpAmount = lerp;
    }

    public void setRad(float Radius)
    {
        radius = Radius;
    }
}
