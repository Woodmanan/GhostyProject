using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private float delay;
    private float timer;
    
    [SerializeField] private Vector3 offset;

    [SerializeField] private GameObject objectToDrop;
    [SerializeField] private float timeOffset;
    [SerializeField] private bool randomTimeOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = -1 * timeOffset;
        if (randomTimeOffset)
        {
            timer += Random.Range(0, delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            GameObject newObj = Instantiate(objectToDrop, transform.position + offset, Quaternion.identity);
            timer = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + offset, .2f);
    }
}
