using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Player;
using Assets.Scripts.Utility;


public class PlayerManager : MonoBehaviour 
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
    public GameObject[] Players = new GameObject[2];
    public Player[] PlayersScripts = new Player[2];
	// Use this for initialization
	void Start () 
    {
        SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            var s = Random.Range(0, SpawnPoints.Length - 1);
            while (s == oldSpawn)
	        {
	         s = Random.Range(0, SpawnPoints.Length - 1);
            }
            Players[i] = Instantiate(PlayerPrefab, SpawnPoints[s].transform.position, Quaternion.identity) as GameObject;
            oldSpawn = s;
            PlayersScripts[i] = Players[i].GetComponent<Player>();
            if (i == 0)
            {
                PlayersScripts[i].isPlayerOne = true;
            }
        }

        //make cameras for players
        if (NumberOfPlayers == 1)
        {
            var ar = Resolution.x/Resolution.y;
            cameraMan.BuildCamera(ar,ref Players[0], CameraFollowSpeed);
        }
        else
            Make2PlayerCam();
	}

    private void Make2PlayerCam()
    {
        throw new System.NotImplementedException();
    }
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            if (!Players[i].activeInHierarchy && PlayersScripts[i].Lives > 0 && !PlayersScripts[i].isRespawning)
            {
                PlayersScripts[i].isRespawning = true;
                StartCoroutine(Respawn(Players[i]));
            }
            else if (PlayersScripts[i].Lives == 0 && !PlayersScripts[i].isRespawning)
            {
                //put game over sectoin here
                //display game over scene
                //options to restart or go to menu
                //
            }
        }
	}

    IEnumerator Respawn(GameObject p)
    {
        p.GetComponent<Player>().Lives--;
        p.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length - 1)].transform.position;
        yield return new WaitForSeconds(RespawnTime);
        p.SetActive(true);
        p.GetComponent<Player>().isRespawning = false;
    }
}
