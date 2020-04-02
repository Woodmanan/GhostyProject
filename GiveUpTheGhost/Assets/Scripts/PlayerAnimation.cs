using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // SCALE: 0.08 all fields
    // POSITION: -0.77 y

    // Static constants
    public int FPS = 12;
    public int FPI = 4;

    // Public fields
    [HideInInspector] public bool facing = true; // True for right, false for left
    [HideInInspector] public bool turning = false;
    [HideInInspector] public bool landing = false;
    [HideInInspector] public string currentAnimId;

    // Component hooks
    private Character ch;
    private SpriteRenderer sr;

    // Animations
    private Sprite[] leftIdleAnim;
    private Sprite[] rightIdleAnim;
    private Sprite[] leftWalkAnim;
    private Sprite[] rightWalkAnim;
    private Sprite[] leftJumpAnim;
    private Sprite[] rightJumpAnim;
    private Sprite[] leftFallAnim;
    private Sprite[] rightFallAnim;
    private Sprite[] leftLandAnim;
    private Sprite[] rightLandAnim;
    private Sprite[] leftRightTurnAnim;
    private Sprite[] rightLeftTurnAnim;

    // Working animation details
    private Sprite[] currentAnim;
    private int currentIndex = 0;
    private int frameCount = 0;
    private float lastFrameStart;
    private bool loop = true;


    /** UNITY SYSTEM ROUTINES **/
    void Awake()
    {
        ch = GetComponentInParent<Character>();
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
                        if (currentAnimId == "leftRightTurn")
                        {
                            // Switch to right idle and clear turning period
                            SetAnimation("rightIdle");
                            turning = false;
                            facing = true; // Right face
                            return;
                        }
                        else if (currentAnimId == "rightLeftTurn")
                        {
                            // Switch to left idle and clear turning period
                            SetAnimation("leftIdle");
                            turning = false;
                            facing = false; // Left face
                            return;
                        }
                        else if (currentAnimId == "leftJump")
                        {
                            // Switch to left falling
                            SetAnimation("leftFall");
                            return;
                        }
                        else if (currentAnimId == "leftLand")
                        {
                            // Switch to left idle
                            SetAnimation("leftIdle");
                            landing = false;
                            return;
                        }
                        else if (currentAnimId == "rightJump")
                        {
                            // Switch to right falling
                            SetAnimation("rightFall");
                            return;
                        }
                        else if (currentAnimId == "rightLand")
                        {
                            // Switch to right idle
                            SetAnimation("rightIdle");
                            landing = false;
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
        else if (anim == "rightIdle")
        {
            currentAnim = rightIdleAnim;
            loop = true;
        }
        else if (anim == "leftRightTurn")
        {
            currentAnim = leftRightTurnAnim;
            loop = false;
            turning = true;
        }
        else if (anim == "rightLeftTurn")
        {
            currentAnim = rightLeftTurnAnim;
            loop = false;
            turning = true;
        }
        else if (anim == "leftWalk")
        {
            currentAnim = leftWalkAnim;
            loop = true;
        }
        else if (anim == "rightWalk")
        {
            currentAnim = rightWalkAnim;
            loop = true;
        }
        else if (anim == "leftJump")
        {
            currentAnim = leftJumpAnim;
            loop = false;
        }
        else if (anim == "leftFall")
        {
            currentAnim = leftFallAnim;
            loop = true;
        }
        else if (anim == "leftLand")
        {
            currentAnim = leftLandAnim;
            landing = true;
            loop = false;
        }
        else if (anim == "rightJump")
        {
            currentAnim = rightJumpAnim;
            loop = false;
        }
        else if (anim == "rightFall")
        {
            currentAnim = rightFallAnim;
            loop = true;
        }
        else if (anim == "rightLand")
        {
            currentAnim = rightLandAnim;
            landing = true;
            loop = false;
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
        Sprite[] sheet = Resources.LoadAll<Sprite>("Sprites/PlayerSheet");
        print(sheet.Length);

        leftIdleAnim = new Sprite[1];
        leftIdleAnim[0] = sheet[0];

        rightIdleAnim = new Sprite[1];
        rightIdleAnim[0] = sheet[2];

        leftRightTurnAnim = new Sprite[1];
        leftRightTurnAnim[0] = sheet[1];

        rightLeftTurnAnim = new Sprite[1];
        rightLeftTurnAnim[0] = sheet[1];

        leftWalkAnim = new Sprite[5];
        for (int i = 4; i <= 7; i++)
        {
            leftWalkAnim[i - 4] = sheet[i];
        }
        leftWalkAnim[4] = sheet[3];

        rightWalkAnim = new Sprite[5];
        for (int i = 9; i <= 12; i++)
        {
            rightWalkAnim[i - 9] = sheet[i];
        }
        rightWalkAnim[4] = sheet[8];

        leftJumpAnim = new Sprite[4];
        for (int i = 14; i <= 17; i++)
        {
            leftJumpAnim[i - 14] = sheet[i];
        }

        leftFallAnim = new Sprite[2];
        leftFallAnim[0] = sheet[18];
        leftFallAnim[1] = sheet[19];

        leftLandAnim = new Sprite[4];
        for (int i = 20; i <= 23; i++)
        {
            leftLandAnim[i - 20] = sheet[i];
        }

        rightJumpAnim = new Sprite[4];
        for (int i = 25; i <= 28; i++)
        {
            rightJumpAnim[i - 25] = sheet[i];
        }

        rightFallAnim = new Sprite[2];
        rightFallAnim[0] = sheet[29];
        rightFallAnim[1] = sheet[30];

        rightLandAnim = new Sprite[4];
        for (int i = 31; i <= 34; i++)
        {
            rightLandAnim[i - 31] = sheet[i];
        }

        SetAnimation("rightIdle");
    }
}
