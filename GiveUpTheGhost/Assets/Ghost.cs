using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ghostMode = false;
    private float speed = .4f;
    private float radius = 4;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GhostMovement();
    }


    private void GhostMovement()
    {
        if (ghostMode)
        {


            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {

                float hMove = Input.GetAxisRaw("Horizontal") * speed;
                float vMove = Input.GetAxisRaw("Vertical") * speed;

                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    
                    SpriteRenderer currentImage = transform.Find("GhostSprite").GetComponent<SpriteRenderer>();
                    currentImage.flipX = false;
                    
                }
                else if(Input.GetAxisRaw("Horizontal") < 0)
                {
                    SpriteRenderer currentImage = transform.Find("GhostSprite").GetComponent<SpriteRenderer>();
                    currentImage.flipX = true;

                }


                Vector3 directionMoved = new Vector3(hMove, vMove, 0);
                Vector3 oppositeMove = new Vector3(0,0,0);
       
                
                if (transform.localPosition.x < 0)
                {
                    if (transform.localPosition.y > 0)
                    {//Quadrant II
                        Debug.Log("Quadrant II");
                        if (Input.GetAxisRaw("Horizontal") > 0)
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                        else
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                        }
                    }
                    else
                    {   //Quadrant III
                        Debug.Log("Quadrant III");
                        if (Input.GetAxisRaw("Horizontal") >= 0)
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {

                            }
                            else
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                        }
                        else
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                            else if(Input.GetAxisRaw("Vertical") == 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                        }
                    }
                }
                else
                {
                    if (transform.localPosition.y > 0)
                    {//Quadrant I
                        Debug.Log("Quadrant I");
                        if (Input.GetAxisRaw("Horizontal") > 0)
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else if(Input.GetAxisRaw("Vertical") == 0)
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                        else
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                    }
                    else
                    {//Quadrant IV
                        Debug.Log("Quadrant IV");
                        if (Input.GetAxisRaw("Horizontal") > 0)
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else if (Input.GetAxisRaw("Vertical") == 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                        else
                        {
                            if (Input.GetAxisRaw("Vertical") > 0)
                            {
                                oppositeMove = new Vector3(-transform.localPosition.normalized.y, transform.localPosition.normalized.x, 0);
                            }
                            else
                            {
                                oppositeMove = new Vector3(transform.localPosition.normalized.y, -transform.localPosition.normalized.x, 0);
                            }
                        }
                    }
                }

               
                

                if ((transform.localPosition + directionMoved).magnitude < radius)
                {
                    transform.Translate(directionMoved);


                }
                else
                {

                    transform.Translate(oppositeMove/13);

                }




            }

        }

    }

}
