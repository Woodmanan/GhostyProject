using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimation : MonoBehaviour
{
    // SCALE: 0.08 all fields
    // POSITION: -0.77 y

    // Static constants
    public int FPS = 24;
    public int FPI = 3;

    // Public fields
    [HideInInspector] public int facing = 0; // -1 left, 0 center, 1 right
    [HideInInspector] public bool turning = false;
    [HideInInspector] public string currentAnimId;

    // Component hooks
    private Ghost gh;
    private SpriteRenderer sr;

    // Animations
    private Sprite[] leftIdleAnim;
    private Sprite[] centerIdleAnim;
    private Sprite[] rightIdleAnim;
    private Sprite[] leftTurnAnim;
    private Sprite[] rightTurnAnim;

    // Working animation details
    private Sprite[] currentAnim;
    private int currentIndex = 0;
    private int frameCount = 0;
    private float lastFrameStart;
    private bool loop = true;


    /** UNITY SYSTEM ROUTINES **/
    void Awake()
    {
        gh = GetComponent<Ghost>();
        sr = GetComponent<SpriteRenderer>();
        LoadAnimations();
    }


    // Update is called once per frame
    void Update()
    {
        while (Time.time - lastFrameStart > 1f / (float)FPS)
        {
            frameCount++;
            if (frameCount >= FPI)
            {
                currentIndex++;
                if (currentIndex >= currentAnim.Length)
                {
                    currentIndex = 0;
                    if (!loop)
                    {
                        // Determine the next animation based on the current
                        if (currentAnimId == "leftTurn" && facing == 0)
                        {
                            SetAnimation("leftIdle");
                            turning = false;
                            facing = -1;
                            return;
                        }
                        else if (currentAnimId == "leftTurn" && facing == -1)
                        {
                            SetAnimation("centerIdle");
                            turning = false;
                            facing = 0;
                            return;
                        }
                        else if (currentAnimId == "rightTurn" && facing == 0)
                        {
                            SetAnimation("rightIdle");
                            turning = false;
                            facing = 1; 
                            return;
                        }
                        else if (currentAnimId == "rightTurn" && facing == 1)
                        {
                            SetAnimation("centerIdle");
                            turning = false;
                            facing = 0;
                            return;
                        }
                    }
                }
                sr.sprite = currentAnim[currentIndex];
                frameCount = 0;
            }  
            lastFrameStart += (1f / (float)FPS);
        }
    }


    /** UNIQUE ROUTINES **/
    public void SetAnimation(string anim)
    {
        if (anim == currentAnimId) return;
        else if (anim == "leftIdle")
        {
            currentAnim = leftIdleAnim;
            loop = true;
        }
        else if (anim == "leftTurn")
        {
            currentAnim = leftTurnAnim;
            loop = false;
            turning = true;
        }
        else if (anim == "centerIdle")
        {
            currentAnim = centerIdleAnim;
            loop = true;
        }
        else if (anim == "rightTurn")
        {
            currentAnim = rightTurnAnim;
            loop = false;
            turning = true;
        }
        else if (anim == "rightIdle")
        {
            currentAnim = rightIdleAnim;
            loop = true;
        }
        else
        {
            print("Player animation " + anim + " not found.");
            return;
        }

        // General handling
        currentAnimId = anim;
        currentIndex = 0;
        frameCount = 0;
        lastFrameStart = Time.time;
        sr.sprite = currentAnim[currentIndex];
    }


    private void LoadAnimations()
    {
        // Get all the sprites from the spritesheet
        Sprite[] sheet = Resources.LoadAll<Sprite>("Sprites/GhostSheet");

        leftIdleAnim = new Sprite[5];
        for (int i = 0; i <= 4; i++)
        {
            leftIdleAnim[i] = sheet[i];
        }

        leftTurnAnim = new Sprite[1];
        leftTurnAnim[0] = sheet[5];

        centerIdleAnim = new Sprite[5];
        for (int i = 6; i <= 10; i++)
        {
            centerIdleAnim[i - 6] = sheet[i];
        }

        rightTurnAnim = new Sprite[1];
        rightTurnAnim[0] = sheet[11];

        rightIdleAnim = new Sprite[5];
        for (int i = 12; i <= 16; i++)
        {
            rightIdleAnim[i - 12] = sheet[i];
        }

        SetAnimation("centerIdle");
    }
}
