/*
Copyright (c) Mr BlckHrtzz
Let The Mind Dominate The Hrtzz
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectibleScript : MonoBehaviour
{

    #region Variables 

    [HideInInspector]
    public bool collectibleIsInGame = true;
    public bool collectibleSpawnAllowed = false;

    GameObject _coin, _heart, _collectible;
    GameObject tempTile;

    float _delayTime = 0;

    #endregion

    #region Unity Functions

    void Start()
    {
        _coin = GameObject.FindGameObjectWithTag(Tags.coin);
        Debug.Log("Got The Coin");
        _heart = GameObject.FindGameObjectWithTag(Tags.health);
        Debug.Log("Got The Heart.");
        _heart.SetActive(false);

    }

    void Update()
    {
        if (!collectibleIsInGame && collectibleSpawnAllowed)
        {
            _delayTime = Random.Range(2, 6);
            if (GameManager.Instance.health < 100f)
            {
                _heart.SetActive(true);
                _collectible = _heart;
                StartCoroutine(SpawnCollectible(_collectible, _delayTime));
            }
            _collectible = _coin;
            StartCoroutine(SpawnCollectible(_collectible, _delayTime));
        }
    }

    //Does the Corresponding functions when Player collides with different Objects.
    private void OnTriggerEnter2D(Collider2D hit)
    {
        switch (hit.gameObject.tag)
        {
            case Tags.coin:
                //Add Score
                GameManager.Instance.CoinUpdate();
                hit.transform.position = new Vector2(hit.transform.position.x, hit.transform.position.y + 25f);
                collectibleIsInGame = false;
                collectibleSpawnAllowed = true;
                break;
            case Tags.health:
                //IncreaseHealth
                GameManager.Instance.HealthUpdate(20);
                hit.transform.position = new Vector2(hit.transform.position.x, hit.transform.position.y + 25f);
                if (GameManager.Instance.health > 100f)
                {
                    _heart.SetActive(false);
                }
                collectibleIsInGame = false;
                collectibleSpawnAllowed = true;
                break;
            case Tags.spikes:
                //Decreses Health By 20;
                GameManager.Instance.HealthUpdate(-10);
                break;
            case Tags.bomb:
                //Decreses Health By 20;
                GameManager.Instance.HealthUpdate(-20);
                break;
            default:
                break;
        }
    }

    #endregion

    #region UserDefined
    // Calculates which collectible to spawn.
    IEnumerator SpawnCollectible(GameObject collectible, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        collectibleSpawnAllowed = false;
        collectibleIsInGame = true;
        tempTile = GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<LevelGenerator>().startTile;
        if (collectible == _heart)
        {
            collectible.transform.position = new Vector2(tempTile.transform.position.x + delayTime * 2, tempTile.transform.position.y + 4.0f);
        }
        else
            collectible.transform.position = new Vector2(tempTile.transform.position.x, tempTile.transform.position.y + 4.0f);

    }
    #endregion

}
