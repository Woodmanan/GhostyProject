using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    
    //Character objects we neec
    private Character body;
    private Ghost ghost;

    // Start is called before the first frame update
    void Start()
    {
        body = GameObject.FindGameObjectWithTag("Body").GetComponent<Character>();
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        if (instance != null)
        {
            Debug.LogError("There are two game managers in the scene!");
        }

        instance = this.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
