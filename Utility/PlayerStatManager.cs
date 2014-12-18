using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;

public class PlayerStatManager : MonoBehaviour 
{
    private PlayerManager playerMan;
    public Button MenuButton;
    public Button ContinueButton;
    public Text LivesTxt, ScoreTxt, DeathsTxt, KillsTxt, EndTxt, TotScoreTxt;
    public int[] Lives ;
    public int[] Score ;
    public int[] Deaths;
    public int[] Kills ;
    public int[] TotScore;

    public List<int> Levels;
    private int currentLevel;
    private bool FirstRun = true;
    private InputHandler menu;
    private Animator anim;


    public void Awake()
    {
        //Screen.orientation = ScreenOrientation.Landscape;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        menu = GameObject.FindGameObjectWithTag("SceneManagers").GetComponent<InputHandler>();
        playerMan = GameObject.FindGameObjectWithTag("SceneManagers").GetComponent<PlayerManager>();
        anim = gameObject.GetComponentInChildren<Animator>();
        //Debug.Log(menu);
        if (FirstRun)
        {
            Lives = new int[2];
            Score = new int[2];
            Deaths = new int[2];
            Kills = new int[2];
            TotScore = new int[2];

            for (int i = 0; i < playerMan.NumberOfPlayers; i++)
            {
                Lives[i] = 3;
                Score[i] = 0;
                Deaths[i] =0;
                Kills[i] = 0;
                TotScore[i] = 0;
            }
        }
        else
        {
                
        }
    }

    public void OnLevelWasLoaded()
    {
        if(Application.loadedLevel == 0)
        {
            Time.timeScale = 1f;
            Destroy(gameObject);
        }
        if(FirstRun) return;
        for (int i = 0; i < playerMan.NumberOfPlayers; i++)
        {
            var p = playerMan.PlayersScripts[i];
           p.Lives  =  Lives[i];
           p.Score  =  Score[i];
           p.Deaths =  Deaths[i];
           p.Kills = Kills[i];
        }
    }

    public void LevelOver(bool isWin, bool isGameComplete)
    {
        //get end level stats
        for (int i = 0; i < playerMan.NumberOfPlayers; i++)
        {
            var p = playerMan.PlayersScripts[i];
            Lives[i] =  p.Lives ;
            Score[i] =  p.Score ;
            Deaths[i] = p.Deaths;
            Kills[i] = p.Kills;
            TotScore[i] += Score[i];
        }
        //set stats
        KillsTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("Player 1 Kills: {0} Player 2 Kills: {1}", Kills[0], Kills[1]) :
               string.Format("Player 1 Kills: {0}", Kills[0]);
        //LivesTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("Player One Lives: {0} Player Two Lives: {1}", Lives[0], Lives[1]) :
        //    string.Format("Player One Lives: {0}", Lives[0]);
        ScoreTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("Player 1 Score: {0} Player 2 Score: {1}", Score[0], Score[1]) :
            string.Format("Player 1 Score: {0}", Score[0]);
        DeathsTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("Player 1 Deaths: {0} Player 2 Deaths: {1}", Deaths[0], Deaths[1]) :
            string.Format("Player 1 Deaths: {0}", Deaths[0]);
        TotScoreTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("P1 Total Score: {0} P2 Total Score: {1}", TotScore[0] , TotScore[1] ) :
            string.Format("Player 1 Score: {0}", TotScore[0]);

        //animate in end level summary
        menu.OpenPanel(anim);

        //set timescale to 0
        StartCoroutine(LevelEndDelay());

        //game lost or complete
        if (!isWin || isGameComplete)
        {
            ContinueButton.enabled = false;
            EndTxt.text = isWin ? "Congratulations You Win!" : "Game Over";
        }
        else // level beaten but game not complete
        {
            ContinueButton.enabled = true;
            EndTxt.text = "Level Complete";
        }
    }

    IEnumerator LevelEndDelay()
    {
        yield return new WaitForSeconds(0.6f);
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject g in objects)
        {
            g.SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void LoadNextLevel()
    {
        //timescale set to 1
        Time.timeScale = 1f;
        //animate end level summary out
        menu.CloseCurrent();
        Application.LoadLevel(++currentLevel);
    }

    public void goMenu()
    {
        Time.timeScale = 1f;
        menu.CloseCurrent();
        Application.LoadLevel(0);
    }

    void UpdateStats()
    {
        for (int i = 0; i < playerMan.NumberOfPlayers; i++)
        {
            var p = playerMan.PlayersScripts[i];
            Lives[i] = p.Lives;
            Score[i] = p.Score;
            Deaths[i] =p.Deaths;
            Kills[i] = p.Kills;
        }
    }
}
