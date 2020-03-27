﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager Instance;

    public static GameManager instance
    {
        get
        {
            if (Instance == null)
            {
                //Make a new instance
                GameObject obj = new GameObject();
                obj.AddComponent<GameManager>();
                obj.name = "Game Manager";
                Instance = obj.GetComponent<GameManager>();
            }

            return Instance;
        }
        set => Instance = value;
    }

    //Character objects we need
    private Character body;
    private Ghost ghost;
    private Vector3 respawnPoint;
    private float lives;

    // Start is called before the first frame update
    void Start()
    {
        //Grab the body, which is always enabled
        GameObject bodytmp = GameObject.FindGameObjectWithTag("Body");
        
        
        //Weird calls, because sometimes the ghost isn't enabled
        //Which causes GameObject.Find to not work
        if (bodytmp)
        {
            body = bodytmp.GetComponent<Character>();
            ghost = body.transform.GetChild(2).GetComponent<Ghost>();
            lives = 3;
            respawnPoint = body.transform.position;
        }

        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void MoveToNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Respawn()
    {
        lives--;
        if (lives == 0)
        {
            RestartLevel();
        }

        body.transform.position = new Vector3(respawnPoint.x, respawnPoint.y, body.transform.position.z);
    }

    public void setCheckpoint(Vector3 position)
    {
        respawnPoint = position;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ping()
    {
        print("Ping!");
    }
}