/*
Copyright (c) Mr BlckHrtzz
Let The Mind Dominate The Hrtzz
*/

using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffectScript : MonoBehaviour
{

    #region Variables 

    GameObject gameLayer;
    GameObject cloudLayerNear;
    GameObject cloudLayerMid;
    GameObject cloudLayerFar;
    GameObject cloudLayerClosest;

    LevelGenerator levelGenerator;
    Animator playerAnimator;
    public float gameSpeed = 5.0f;
    float gameStartTime = 0;
    [HideInInspector]
    public bool gameBegining;
    float timeToStartGame;
    float timer;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        timeToStartGame = 3.0f;
        gameBegining = true;
    }
    void Start()
    {
        playerAnimator = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Animator>();
        gameLayer = GameObject.FindGameObjectWithTag(Tags.gameLayer);
        cloudLayerFar = GameObject.FindGameObjectWithTag(Tags.cloudLayerFar);
        cloudLayerMid = GameObject.FindGameObjectWithTag(Tags.cloudLayerMid);
        cloudLayerNear = GameObject.FindGameObjectWithTag(Tags.cloudLayerNear);
        cloudLayerClosest = GameObject.FindGameObjectWithTag(Tags.cloudLayerClosest);
    }

    void Update()
    {
        //Returns is Game has not started.
        if (gameBegining)
        {
            if (timer > timeToStartGame)
            {
                gameBegining = false;
                gameStartTime = Time.time;
                playerAnimator.SetBool("IsGameStart", true);
                Debug.Log("HelloWorld");
            }
            else
                timer += Time.deltaTime;
            return;
        }

        if (GameManager.Instance.isPlayerDead)
        {
            gameSpeed = 0;
            return;
        }
        //Moves layers at different speed.
        gameLayer.transform.position = new Vector2(gameLayer.transform.position.x - gameSpeed * Time.deltaTime, 0f);                                 //Moves layers at different speed.
        cloudLayerFar.transform.position = new Vector2(cloudLayerFar.transform.position.x - gameSpeed * (Time.deltaTime / 5), 0);                    //Moves layers at different speed.
        cloudLayerMid.transform.position = new Vector2(cloudLayerMid.transform.position.x - gameSpeed * (Time.deltaTime / 4), 0);                    //Moves layers at different speed.
        cloudLayerNear.transform.position = new Vector2(cloudLayerNear.transform.position.x - gameSpeed * (Time.deltaTime / 3), 0);                  //Moves layers at different speed.
        cloudLayerClosest.transform.position = new Vector2(cloudLayerClosest.transform.position.x - gameSpeed * (Time.deltaTime / 2), 0);            //Moves layers at different speed.
        Debug.Log(gameSpeed);

        #region Endless Background.
        foreach (Transform child in cloudLayerFar.transform)
        {
            if (child.position.x < -25.57f)
            {
                child.transform.position = new Vector2(25.57f, child.transform.position.y);
            }
        }
        foreach (Transform child in cloudLayerMid.transform)
        {
            if (child.position.x < -25.6f)
            {
                child.transform.position = new Vector2(25.6f, child.transform.position.y);
            }
        }

        foreach (Transform child in cloudLayerNear.transform)
        {
            if (child.position.x < -25.5f)
            {
                child.transform.position = new Vector2(25.5f, child.transform.position.y);
            }
        }
        foreach (Transform child in cloudLayerClosest.transform)
        {
            if (child.position.x < -14.5f)
            {
                child.transform.position = new Vector2(14.5f, Random.Range(-2f, 4.5f));
            }
        }
    }
    #endregion

    #endregion


}
