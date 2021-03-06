﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Player;
using Assets.Scripts.Utility;


public class VSPlayerMan : MonoBehaviour
{

    public CameraManager cameraMan;
    public Vector2 Resolution = new Vector2(1920, 1080);
    public bool isScaled;
    public float CameraFollowSpeed;

    public GameObject PlayerPrefab;
    public int NumberOfPlayers;
    public int RespawnTime;
    private GameObject[] SpawnPoints;
    public int oldSpawn = -1;
    private GameObject[] Players = new GameObject[2];
    public Player[] PlayersScripts = new Player[2];
    private Vector2[] Borders = new Vector2[4];
    private bool isEndScene;
    private GameObject _gameManager;
    // Use this for initialization
    void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager");
        var g = _gameManager.GetComponent<PlayerStatManager>();
        NumberOfPlayers = g.numberOfPlayers;
        SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            var s = Random.Range(0, SpawnPoints.Length);
            while (s == oldSpawn)
            {
                s = Random.Range(0, SpawnPoints.Length);
            }
            Players[i] = Instantiate(PlayerPrefab, SpawnPoints[s].transform.position, Quaternion.identity) as GameObject;
            oldSpawn = s;
            PlayersScripts[i] = Players[i].GetComponent<Player>();
            if (i == 0)
            {
                PlayersScripts[i].isPlayerOne = true;
            }

            var p = PlayersScripts[i];
            p.isVersus = true;
            p.Lives = g.Lives[i];
            p.Deaths = g.Deaths[i];
            p.Kills = g.Kills[i];
        }
    }
    void Start()
    {
        var ar = Resolution.x / Resolution.y;
        cameraMan.BuildVSCamera(ar, Players, CameraFollowSpeed);

        #region find border limits
        var Bl = GameObject.FindGameObjectsWithTag("BorderLimits");
        if (Bl == null || Bl.Length < 4) Debug.LogError("borders not found");
        else
        {
            for (int i = 0; i < 4; i++)
            {
                Borders[i] = Bl[i].transform.position;
            }

            //sort borders int left top right bottom order
            Vector2[] templtrb = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                templtrb[i] = Borders[i];
            }
            //find left edge
            Vector2 tempV = templtrb[0];
            for (int i = 1; i < 4; i++)
            {
                if (templtrb[i].x < tempV.x)
                    tempV = templtrb[i];
            }
            Borders[0] = tempV;
            //find top 
            tempV = templtrb[0];
            for (int i = 1; i < 4; i++)
            {
                if (templtrb[i].y > tempV.y)
                    tempV = templtrb[i];
            }
            Borders[1] = tempV;
            //find right
            tempV = templtrb[0];
            for (int i = 1; i < 4; i++)
            {
                if (templtrb[i].x > tempV.x)
                    tempV = templtrb[i];
            }
            Borders[2] = tempV;
            //find bottom
            tempV = templtrb[0];
            for (int i = 1; i < 4; i++)
            {
                if (templtrb[i].y < tempV.y)
                    tempV = templtrb[i];
            }
            Borders[3] = tempV;

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                PlayersScripts[i].Borders = Borders;
            }
        }
        #endregion
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            if (!Players[i].activeInHierarchy && PlayersScripts[i].Lives > 0 && !PlayersScripts[i].isRespawning)
            {
                PlayersScripts[i].isRespawning = true;
                StartCoroutine(Respawn(Players[i]));
            }
            else if (!isEndScene && PlayersScripts[i].Lives == 0 && !PlayersScripts[i].isRespawning && !Players[i].activeInHierarchy)
            {
                var o = i == 0 ? 1 : 0;
                isEndScene = true;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStatManager>().VSLevelOver(o);
            }
        }

    }

    IEnumerator Respawn(GameObject p)
    {
        p.GetComponent<Player>().Lives--;
        var sp = Random.Range(0, SpawnPoints.Length - 1);
        p.transform.position = SpawnPoints[sp].transform.position;
        yield return new WaitForSeconds(RespawnTime);
        p.SetActive(true);
        p.GetComponent<Player>().isRespawning = false;
        yield return new WaitForSeconds(0.5f);
    }
}
