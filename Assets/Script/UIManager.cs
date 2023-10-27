using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle to text 
    [SerializeField]
    private Text _scoreText;
    
    [SerializeField]
    private Image _LivesImg; 

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameoverActive;

    [SerializeField]
    private Text _resetText;

    private GameManager _gameManager; 

    

    // Start is called before the first frame update
    void Start()
    {    
        //assign text component to the handle
        _scoreText.text = "Score: " + 0;
        _gameoverActive.gameObject.SetActive(false);
        _resetText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString(); 
    }

    public void UpdateLives(int currentLives)
    {
        //display img Sprite
        //give it new one based on the currentLives index
        _LivesImg.sprite = _liveSprites[currentLives]; 

        if (currentLives == 0 )
        {
          GameOver();

        }

    void GameOver()
    {
          _gameoverActive.gameObject.SetActive(true);
            StartCoroutine(GameOverBlinkRoutine());

            _resetText.gameObject.SetActive(true);
    }

    IEnumerator GameOverBlinkRoutine()
    {
        while(true)
        {
            _gameoverActive.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameoverActive.text = " ";
            yield return new WaitForSeconds(0.5f);
        }
    }

   



    }

   
}
