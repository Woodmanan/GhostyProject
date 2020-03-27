using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossfightControls : MonoBehaviour
{
    private PlayableDirector director;
    [SerializeField] private TimelineAsset startClip;
    [SerializeField] private TimelineAsset endClip;

    private bool fightStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Body") && !fightStarted)
        {
            director.Play(startClip);
            fightStarted = true;
        }
    }

    public void endFight()
    {
        director.Play(endClip);
    }
}
