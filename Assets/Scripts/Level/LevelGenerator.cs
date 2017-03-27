/*
Copyright (c) Mr BlckHrtzz
Let The Mind Dominate The Hrtzz
*/

using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    #region Variables 
    GameObject availableTiles;              //For Creating The pool of tiles.          
    GameObject tempTile;                    //tempTile for spawning new tile.
    CollectibleScript collectibleScript;
    [HideInInspector]
    public GameObject startTile;            //The position of new tile to be placed.
    GameObject newEnemy;
    GameObject gameLayer;

    float startPosY = 0;

    const float tileWidth = 1.28f;          //Width of each Tile.
    const float tileHeight = tileWidth * 3; //Height of Each tile.

    float spawnHeight = 0;                  //Random Height for spawning new Tile.
    int middleCounter = 0;
    int blankCounter = 0;
    float outOfAreaX;                       //The left bound of camera.
    bool enemyAdded;

    string lastTileType;

    #endregion

    #region Unity Functions

    void Awake()
    {
        collectibleScript = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<CollectibleScript>();

        gameLayer = GameObject.FindGameObjectWithTag(Tags.gameLayer);
        availableTiles = GameObject.FindGameObjectWithTag(Tags.tiles);
        startTile = gameLayer.transform.FindChild("GroundLeft").gameObject;
        startPosY = startTile.transform.position.y;
        outOfAreaX = startTile.transform.position.x - 5.0f;

        //Instantiating Objects inside the Pool.
        #region Pool Isntantiat Region
        for (int i = 0; i < 30; i++)
        {
            GameObject tempGLeft = Instantiate(Resources.Load("GroundLeft", typeof(GameObject))) as GameObject;
            tempGLeft.transform.parent = availableTiles.transform.FindChild("groundLeft").transform;
            tempGLeft.transform.position = Vector2.zero;

            GameObject tempGMid = Instantiate(Resources.Load("GroundMiddle", typeof(GameObject))) as GameObject;
            tempGMid.transform.parent = availableTiles.transform.FindChild("groundMiddle").transform;
            tempGMid.transform.position = Vector2.zero;

            GameObject tempGRight = Instantiate(Resources.Load("GroundRight", typeof(GameObject))) as GameObject;
            tempGRight.transform.parent = availableTiles.transform.FindChild("groundRight").transform;
            tempGRight.transform.position = Vector2.zero;

            GameObject tempPLeft = Instantiate(Resources.Load("PlatformLeft", typeof(GameObject))) as GameObject;
            tempPLeft.transform.parent = availableTiles.transform.FindChild("platformLeft").transform;
            tempPLeft.transform.position = Vector2.zero;

            GameObject tempPMid = Instantiate(Resources.Load("PlatformMiddle", typeof(GameObject))) as GameObject;
            tempPMid.transform.parent = availableTiles.transform.FindChild("platformMiddle").transform;
            tempPMid.transform.position = Vector2.zero;

            GameObject tempPRight = Instantiate(Resources.Load("PlatformRight", typeof(GameObject))) as GameObject;
            tempPRight.transform.parent = availableTiles.transform.FindChild("platformRight").transform;
            tempPRight.transform.position = Vector2.zero;

            GameObject tempBlank = Instantiate(Resources.Load("Blank", typeof(GameObject))) as GameObject;
            tempBlank.transform.parent = availableTiles.transform.FindChild("blank").transform;
            tempBlank.transform.position = Vector2.zero;
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject tempSpike = Instantiate(Resources.Load("Spike", typeof(GameObject))) as GameObject;
            tempSpike.transform.parent = availableTiles.transform.FindChild("spike").transform;
            tempSpike.transform.position = Vector2.zero;

            GameObject tempBomb = Instantiate(Resources.Load("Bomb", typeof(GameObject))) as GameObject;
            tempBomb.transform.parent = availableTiles.transform.FindChild("bomb").transform;
            tempBomb.transform.position = Vector2.zero;
        }
        #endregion

        availableTiles.transform.position = new Vector2(-30f, 0);       //Assining arbitary position to pool.
        SetupScene();                   //Creating the starting Scene.
    }

    void FixedUpdate()
    {

        //Condition For Sending Tiles which are out of bouds to Available tiles Pool.
        #region LevelPoolRegion
        foreach (Transform child in gameLayer.transform)
        {
            if (child.position.x < outOfAreaX)
            {
                switch (child.tag)
                {
                    case Tags.groundLeft:
                        child.gameObject.transform.position = availableTiles.transform.FindChild("groundLeft").transform.position;
                        child.gameObject.transform.parent = availableTiles.transform.FindChild("groundLeft").transform;
                        break;
                    case Tags.groundMiddle:
                        child.gameObject.transform.position = availableTiles.transform.FindChild("groundMiddle").transform.position;
                        child.gameObject.transform.parent = availableTiles.transform.FindChild("groundMiddle").transform;
                        break;
                    case Tags.groundRight:
                        child.gameObject.transform.position = availableTiles.transform.FindChild("groundRight").transform.position;
                        child.gameObject.transform.parent = availableTiles.transform.FindChild("groundRight").transform;
                        break;
                    case Tags.platformLeft:
                        child.gameObject.transform.position = availableTiles.transform.FindChild("platformLeft").transform.position;
                        child.gameObject.transform.parent = availableTiles.transform.FindChild("platformLeft").transform;
                        break;
                    case Tags.platformMiddle:
                        child.gameObject.transform.position = availableTiles.transform.FindChild("platformMiddle").transform.position;
                        child.gameObject.transform.parent = availableTiles.transform.FindChild("platformMiddle").transform;
                        break;
                    case Tags.platformRight:
                        child.gameObject.transform.position = availableTiles.transform.FindChild("platformRight").transform.position;
                        child.gameObject.transform.parent = availableTiles.transform.FindChild("platformRight").transform;
                        break;
                    case Tags.blank:
                        child.gameObject.transform.position = availableTiles.transform.FindChild("blank").transform.position;
                        child.gameObject.transform.parent = availableTiles.transform.FindChild("blank").transform;
                        break;
                    case Tags.spikes:
                        child.gameObject.transform.position = availableTiles.transform.FindChild("spike").transform.position;
                        child.gameObject.transform.parent = availableTiles.transform.FindChild("spike").transform;
                        break;
                    case Tags.bomb:
                        child.gameObject.transform.position = availableTiles.transform.FindChild("bomb").transform.position;
                        child.gameObject.transform.parent = availableTiles.transform.FindChild("bomb").transform;
                        break;
                    case Tags.coin:
                        collectibleScript.collectibleIsInGame = false;
                        collectibleScript.collectibleSpawnAllowed = true;
                        break;
                    case Tags.health:
                        if (child.gameObject.activeSelf)
                        {
                            collectibleScript.collectibleSpawnAllowed = true;
                            collectibleScript.collectibleIsInGame = false;
                        }
                        break;
                    default:
                        Destroy(child.gameObject);
                        break;
                }
            }
        }
        #endregion         

        //Spawning new Tiles from available Tile Pool.
        if (gameLayer.transform.childCount < 30)
        {
            SpawnNewTiles();        //Function for spawning new tiles;
        }

    }

    #endregion

    #region UserDefined
    //Sets up The First Scene at begining.
    void SetupScene()
    {
        for (int i = 0; i < 25; i++)
        {
            TileSetup("gMid");
        }
        TileSetup("gRight");
    }
    
    //Sets the new tile and it`s position.
    void TileSetup(string tileType)
    {
        switch (tileType)
        {
            case "gLeft":
                tempTile = availableTiles.transform.FindChild("groundLeft").transform.GetChild(0).gameObject;
                break;
            case "gMid":
                tempTile = availableTiles.transform.FindChild("groundMiddle").transform.GetChild(0).gameObject;
                break;
            case "gRight":
                tempTile = availableTiles.transform.FindChild("groundRight").transform.GetChild(0).gameObject;
                break;
            case "pLeft":
                tempTile = availableTiles.transform.FindChild("platformLeft").transform.GetChild(0).gameObject;
                break;
            case "pMid":
                tempTile = availableTiles.transform.FindChild("platformMiddle").transform.GetChild(0).gameObject;
                break;
            case "pRight":
                tempTile = availableTiles.transform.FindChild("platformRight").transform.GetChild(0).gameObject;
                break;
            case "blank":
                tempTile = availableTiles.transform.FindChild("blank").transform.GetChild(0).gameObject;
                break;
        }

        tempTile.transform.parent = gameLayer.transform;
        tempTile.transform.position = new Vector2(startTile.transform.position.x + tileWidth, startPosY + (spawnHeight * tileHeight));
        startTile = tempTile;
        lastTileType = tileType;
    }

    //Spawns Tile Of the Required Type;
    void SpawnNewTiles()
    {
        if (blankCounter > 0)
        {
            TileSetup("blank");
            blankCounter--;
            return;
        }

        if (middleCounter > 0)
        {
            if (spawnHeight >= 0.5)
            {
                SpawnRandomEnemy(1.92f);
                TileSetup("pMid");
            }
            else if (spawnHeight == 0)
            {
                SpawnRandomEnemy(2.56f);
                TileSetup("gMid");
            }
            middleCounter--;
            return;
        }
        enemyAdded = false;

        if (lastTileType == "blank")
        {
            ChangeSpawnHeight();
            if (spawnHeight >= 0.5)
            {
                TileSetup("pLeft");
            }
            else if (spawnHeight == 0)
            {
                TileSetup("gLeft");
            }
            middleCounter = Random.Range(1, 9);
        }
        else if (lastTileType == "gRight" || lastTileType == "pRight")
        {
            blankCounter = Random.Range(2, 3);
        }
        else if (lastTileType == "gMid" || lastTileType == "pMid")
        {
            if (spawnHeight == 0)
            {
                TileSetup("gRight");
            }
            else if (spawnHeight > 0)
            {
                TileSetup("pRight");
            }
        }

    }

    //Function For assigning random Height to tiles.
    void ChangeSpawnHeight()
    {
        int newSpawnHeight = Random.Range(0, 2);
        if (newSpawnHeight > spawnHeight)
            spawnHeight += 0.5f;
        else if (newSpawnHeight < spawnHeight)
            spawnHeight -= 0.5f;
    }

    //Function for spawning Random enemies.
    void SpawnRandomEnemy(float enemySpawnHeight)
    {
        if (enemyAdded)
            return;
        if (Random.Range(0, 4) == 1)
        {
            if (Random.Range(0, 5) < 3)
            {
                newEnemy = availableTiles.transform.FindChild("spike").transform.GetChild(0).gameObject;
            }
            else
                newEnemy = availableTiles.transform.FindChild("bomb").transform.GetChild(0).gameObject;
            newEnemy.transform.parent = gameLayer.transform;
            newEnemy.transform.position = new Vector2(startTile.transform.position.x + tileWidth, startPosY + (tileHeight * spawnHeight) + enemySpawnHeight);
            enemyAdded = true;
        }
    }
    #endregion

}
