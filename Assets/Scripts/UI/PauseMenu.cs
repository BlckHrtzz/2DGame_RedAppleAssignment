/*
Copyright (c) Mr BlckHrtzz
Let The Mind Dominate The Hrtzz
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    #region Variables 
    public Text coinText;               //Public refernce for corresponding Text Fields.
    public Text scoreText;              //Public refernce for corresponding Text Fields.
    public Text hiScoreText;            //Public refernce for corresponding Text Fields.
    #endregion

    #region Unity Functions

    void Awake()
    {
        if(coinText==null||scoreText==null||hiScoreText==null)
        {
            Debug.LogError("Please Attach Following Component");
        }
        gameObject.SetActive(false);        
    }
    #endregion

    #region UserDefined

    public void ShowPauseMenu(int coin, float score, int highScore)
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        coinText.text = coin.ToString();
        scoreText.text = "Score\n" + (int)score;
        hiScoreText.text = "Hi-Score\n" + highScore;
    }

    public void ResumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    #endregion

}
