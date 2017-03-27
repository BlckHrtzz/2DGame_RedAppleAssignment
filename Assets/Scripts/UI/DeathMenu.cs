/*
Copyright (c) Mr BlckHrtzz
Let The Mind Dominate The Hrtzz
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    #region Variables 
    public Text coinText;
    public Text scoreText;
    public Text hiScoreText;
    bool isShowing;
    float transition;
    public Image backGround;
    #endregion

    #region Unity Functions

    void Awake()
    {
        if (coinText == null || scoreText == null || hiScoreText == null)
        {
            Debug.LogError("Please Attach Following Component");
        }
        gameObject.SetActive(false);
       
    }

    void Update()
    {
        if (!isShowing)
            return;
        transition += Time.deltaTime;
        backGround.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);      //Transition from Transparent to black.

    }
    
    #endregion

    #region UserDefined
    public void ShowGameOver(int coin, float score, int highScore)
    {
        gameObject.SetActive(true);
        int coinScore = coin * 10;
        int finalScore = (int)score + coinScore;
        if (finalScore > highScore)
            highScore = finalScore;

        coinText.text = coin + "  X 10 = " + coinScore;
        scoreText.text = "Score : " + finalScore;
        hiScoreText.text = "Hi-Score : " + highScore;
        isShowing = true;
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
        
    }

    
    #endregion

}
