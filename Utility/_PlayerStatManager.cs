using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;

public class _PlayerStatManager : MonoBehaviour 
{
    public PlayerManager playerMan;
    public int[] Lives ;
    public int[] Score ;
    public int[] Deaths;
    public int[] Kills ;

    public List<int> Levels;
    private int currentLevel;
    private bool FirstRun = true;
    private InputHandler menu;
    private Animator anim;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        menu = GameObject.FindGameObjectWithTag("SceneManagers").GetComponent<InputHandler>();
        anim = gameObject.GetComponentInChildren<Animator>();
        if (FirstRun)
        {
            Lives = new int[2];
            Score = new int[2];
            Deaths = new int[2];
            Kills = new int[2];

            for (int i = 0; i < playerMan.NumberOfPlayers; i++)
            {
                Lives[i] = 3;
                Score[i] = 0;
                Deaths[i] =0;
                Kills[i] = 0;
            }
        }
    }

    public void OnLevelWasLoaded()
    {
        if(FirstRun) return;
        for (int i = 0; i < playerMan.NumberOfPlayers; i++)
        {
            playerMan.PlayersScripts[i].Lives =  Lives[i];
        }
    }

    public void LevelOver(bool isWin, bool isGameComplete)
    {

        //animate in end level summary
        menu.OpenPanel(anim);
        //set timescale to 0
        Time.timeScale = 0f;
        //if not win
        //show stats
        //continue not shown

        //if win && not game complete
        //show stats
        //calc additional lives
        // show quit and continue

        //game complete
        //show stats
        //show quit
    }

    public void LoadNextLevel()
    {
        //timescale set to 1
        Time.timeScale = 1f;
        //animate end level summary out
        Application.LoadLevel(++currentLevel);
    }
}
