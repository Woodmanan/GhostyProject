using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLerp : MonoBehaviour
{
    [SerializeField] private Color first;

    [SerializeField] private Color second;

    [SerializeField] private float lerpTime;
    
    private Camera cam;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        StartCoroutine(moveBetween(first, second, lerpTime));
    }

    IEnumerator moveBetween(Color one, Color two, float lerp)
    {
        float timer = 0;
        cam.backgroundColor = one;
        yield return null;
        while (timer < lerp)
        {
            cam.backgroundColor = Color.Lerp(one, two, timer / lerp);
            timer += Time.deltaTime;
            yield return null;
        }

        cam.backgroundColor = two;
        StartCoroutine(moveBetween(two, one, lerp));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
