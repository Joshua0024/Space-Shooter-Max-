using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{

    [SerializeField]
    private bool _isGameOver;

    private bool _escGame;
    
    private void Update()
    {
          
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true )
        {
            SceneManager.LoadScene(1);
        }

        // if the esc key is press 
        // quit application 

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    // Update is called once per frame
   
}
