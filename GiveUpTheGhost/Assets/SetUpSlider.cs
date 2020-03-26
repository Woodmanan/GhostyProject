using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpSlider : MonoBehaviour
{
    [SerializeField] private float distanceAbove = 1;
    [SerializeField] private float distanceBelow = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        SliderJoint2D slide = GetComponent<SliderJoint2D>();
        slide.useLimits = true;
        slide.connectedAnchor = new Vector2(transform.position.x,  transform.position.y);
        slide.angle = -90;
        JointTranslationLimits2D lims = new JointTranslationLimits2D();
        lims.max = distanceAbove;
        lims.min = -1 * distanceBelow;
        slide.limits = lims;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 above = transform.position + Vector3.up * distanceAbove;
        Vector3 below = transform.position + Vector3.down * distanceBelow;
        Gizmos.DrawWireSphere(above, .2f);
        Gizmos.DrawWireSphere(below, .2f);
        Gizmos.DrawLine(above, below);
    }
}
