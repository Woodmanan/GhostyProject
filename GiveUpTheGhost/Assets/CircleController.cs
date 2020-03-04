using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    private Material mat;
    [SerializeField] private float radius;

    [SerializeField] private float thiccnes;
    [SerializeField] private float lerpAmount;
    private static readonly int RadOne = Shader.PropertyToID("_RadOne");
    private static readonly int RadTwo = Shader.PropertyToID("_RadTwo");
    private static readonly int Pos = Shader.PropertyToID("_Pos");

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        lerpAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float innerRad = radius;
        float outerRad = radius + thiccnes;
        mat.SetFloat(RadOne, outerRad * lerpAmount);
        mat.SetFloat(RadTwo, innerRad * lerpAmount);
        mat.SetVector(Pos, transform.position);
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
