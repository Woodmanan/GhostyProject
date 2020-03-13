using System.Collections;
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

    //Character objects we neec
    private Character body;
    private Ghost ghost;

    // Start is called before the first frame update
    void Start()
    {
        //Grab the body, which is always enabled
        body = GameObject.FindGameObjectWithTag("Body").GetComponent<Character>();
        
        //Weird calls, because sometimes the ghost isn't enabled
        //Which causes GameObject.Find to not work
        ghost = body.transform.GetChild(2).GetComponent<Ghost>();
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
