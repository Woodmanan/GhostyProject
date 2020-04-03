using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class PossesTexControls : MonoBehaviour
{
    private Material mat;
    private Ghost ghost;

    [SerializeField] private float wiggleSpeed;

    private float offset;
    [SerializeField] private float maxGreen;
    [SerializeField] private float maxOffset;
    public Color floatColor;

    private static readonly int OffAmt = Shader.PropertyToID("_OffAmt");

    private float amt;
    private static readonly int GrnAmt = Shader.PropertyToID("_GrnAmt");
    private static readonly int Offset = Shader.PropertyToID("_Offset");
    private static readonly int Color = Shader.PropertyToID("_Color");

    // Start is called before the first frame update
    void Start()
    {
        amt = 0;
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (ghost.ghostMode && ghost.gameObject.active)
        {
            amt = Mathf.Lerp(amt, 1, .05f);
            if (amt > .95)
            {
                amt = 1;
            }
        }
        else
        {
            amt = Mathf.Lerp(amt, 0, .05f);
            if (amt < .05)
            {
                amt = 0;
                offset = 0;
            }
        }

        

        offset += wiggleSpeed * Time.deltaTime;
        
        mat.SetFloat(OffAmt, offset);
        mat.SetFloat(GrnAmt, maxGreen * amt);
        mat.SetFloat(Offset, maxOffset * amt);
        mat.SetColor(Color, floatColor);
    }

    public void reset()
    {
        mat.SetFloat(OffAmt, 0);
        mat.SetFloat(GrnAmt, 0);
        mat.SetFloat(Offset, 0);
    }

}
