/*
Copyright (c) Mr BlckHrtzz
Let The Mind Dominate The Hrtzz
*/

using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    #region Variables 
    GameObject player;
    ParallaxEffectScript parallaxEffectScript;      //Reference for the corresponding Script.
    public DeathMenu deathMenuScript;               //Reference for the corresponding Script.
    public PauseMenu pauseMenuScript;               ////Reference for the corresponding Script.

    float score;
    int highScore;

    public float starthealth = 150;
    [HideInInspector]
    public float health;
    [HideInInspector]
    public int coin;
    [HideInInspector]
    public bool isPlayerDead;

    public Text totalCoins;                         //Reference For corresponding text field.
    public Text scoreText;                          //Reference For corresponding text field.
    public Text highScoreText;                      //Reference For corresponding text field.
    public Image currentHealthBar;                  //Reference For corresponding text field.

    int scoreToNextLevel;                           // Score to reach next Level or say Difficulty.
    [HideInInspector]
    public int currentDifficulty;
    int maxDifficulty;

    #endregion

    #region Unity Functions
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        parallaxEffectScript = GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<ParallaxEffectScript>();
        player = GameObject.FindGameObjectWithTag(Tags.player);

        health = starthealth;
        coin = 0;
        score = 0;
        highScore = PlayerPrefs.GetInt("HighScore", highScore);

        scoreToNextLevel = 10;
        currentDifficulty = 1;
        maxDifficulty = 10;

        totalCoins.text = coin.ToString();
        scoreText.text = "Score : " + score;
        highScoreText.text = "HighScore  : " + highScore;

    }

    void Update()
    {
        if (parallaxEffectScript.gameBegining == true)      // returns if game has not started.
            return;

        isPlayerDead = IsPlayerDead();                      //Call Die Coroutine if player is dead.
        if (isPlayerDead)
        {
            StartCoroutine(Die());
            return;
        }


        score += Time.deltaTime * currentDifficulty;        //Increases the score in rscpect to current difficulty.
        //Debug.Log(score);
        scoreText.text = "Score : " + (int)score;           //For Displaying The score on UI.

        //Condition To increase the dDifficulty.
        if (score > scoreToNextLevel)
        {
            GoToNextLevel();
        }

        //Setting Up the High Score.
        if ((int)score > highScore)
        {
            highScore = (int)score;
        }
        highScoreText.text = "Hi-Score : " + highScore;   //For Displaying The Hi-score on UI.

        //Condition For Pausing Game.
        if (Input.GetButtonDown("Cancel"))
        {
            PauseTheGame();
        }
    }

    #endregion

    #region UserDefined
    //Updates The Coins.
    public void CoinUpdate()
    {
        coin++;
        totalCoins.text = coin.ToString();
    }

    //Updates The Health
    public void HealthUpdate(int h)
    {
        health += h;
        currentHealthBar.fillAmount = health / starthealth;
    }

    //Checks if Player is Dead.
    public bool IsPlayerDead()
    {
        if (health <= 0 || player.transform.position.y <= -4)
        {
            return true;
        }
        else
            return false;
    }

    //Sets the New Difficulty.
    void GoToNextLevel()
    {
        if (currentDifficulty == maxDifficulty)
            return;
        scoreToNextLevel *= 2;
        currentDifficulty++;
        parallaxEffectScript.gameSpeed += 0.5f;
        Debug.Log("Difficulty " + currentDifficulty);
    }

    //when Player is dead.
    IEnumerator Die()
    {
        player.GetComponent<Animator>().SetBool("IsDead", true);
        yield return new WaitForSeconds(1.5f);
        deathMenuScript.ShowGameOver(coin, score, highScore);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Animator>().enabled = false;
    }

    //Pauses The Game While pasing the Required Values
    public void PauseTheGame()
    {
        pauseMenuScript.ShowPauseMenu(coin, score, highScore);
    }
    #endregion


}
