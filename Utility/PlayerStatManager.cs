using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;

public class PlayerStatManager : MonoBehaviour 
{
    private PlayerManager playerMan;
    private VSPlayerMan vsPlayerMan;
    public Button MenuButton;
    public Button ContinueButton;
    public Text LivesTxt, ScoreTxt, DeathsTxt, KillsTxt, EndTxt, TotScoreTxt;
    public int numberOfPlayers;
    public int[] Lives ;
    public int[] Score ;
    public float[] VSScore;
    public int[] Deaths;
    public int[] Kills ;
    public int[] TotScore;

    public int[] Levels;
    public int currentLevel;
    private bool FirstRun = true;
    private InputHandler menu;
    private Animator anim;

    public Text LoadingText;
    public Slider ProgressBar;
    public Image bg;

    private bool isLoading;


    public void Awake()
    {
        //Screen.orientation = ScreenOrientation.Landscape;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        menu = gameObject.GetComponent<InputHandler>();
        
        anim = gameObject.GetComponentInChildren<Animator>();
        //Debug.Log(menu);
        if (FirstRun)
        {
            Lives = new int[2];
            Score = new int[2];
            VSScore = new float[2];
            Deaths = new int[2];
            Kills = new int[2];
            TotScore = new int[2];

            for (int i = 0; i < numberOfPlayers; i++)
            {
                Lives[i] = 3;
                Score[i] = 0;
                VSScore[i] = 0;
                Deaths[i] =0;
                Kills[i] = 0;
                TotScore[i] = 0;
            }
            FirstRun = false;
        }

    }

    public void OnLevelWasLoaded()
    {
        isLoading = false;
        //Debug.Log("levelloaded ran");
        gameObject.SetActive(true);
        if(Application.loadedLevel == 1)
        {
            //Time.timeScale = 1f;
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        if(FirstRun) return;
        menu = gameObject.GetComponent<InputHandler>();
        if (GameObject.FindGameObjectWithTag("SceneManagers") != null)
        {
            if (GameObject.FindGameObjectWithTag("SceneManagers").GetComponent<PlayerManager>() != null)
                playerMan = GameObject.FindGameObjectWithTag("SceneManagers").GetComponent<PlayerManager>();
            if (GameObject.FindGameObjectWithTag("SceneManagers").GetComponent<VSPlayerMan>() != null)
                vsPlayerMan = GameObject.FindGameObjectWithTag("SceneManagers").GetComponent<VSPlayerMan>();
        }
        LoadingText.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
    }

    public void LevelOver(bool isWin)
    {
        bool isGameComplete = currentLevel == Levels.Length;
        //get end level stats
        for (int i = 0; i < playerMan.NumberOfPlayers; i++)
        {
            var p = playerMan.PlayersScripts[i];
            Score[i] = p.Score;
            p.Lives += Score[i] / 3000;
            Lives[i] =  p.Lives ;
            Deaths[i] = p.Deaths;
            Kills[i] = p.Kills;
            TotScore[i] += Score[i];
        }
        //set stats
        KillsTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("Player 1 Kills: {0} \nPlayer 2 Kills: {1}", Kills[0], Kills[1]) :
               string.Format("Player 1 Kills: {0}", Kills[0]);
        //LivesTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("Player One Lives: {0} Player Two Lives: {1}", Lives[0], Lives[1]) :
        //    string.Format("Player One Lives: {0}", Lives[0]);
        ScoreTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("Player 1 Score: {0} \nPlayer 2 Score: {1}", Score[0], Score[1]) :
            string.Format("Player 1 Score: {0}", Score[0]);
        DeathsTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("Player 1 Deaths: {0} \nPlayer 2 Deaths: {1}", Deaths[0], Deaths[1]) :
            string.Format("Player 1 Deaths: {0}", Deaths[0]);
        TotScoreTxt.text = playerMan.NumberOfPlayers > 1 ? string.Format("P1 Total Score: {0} \nP2 Total Score: {1}", TotScore[0] , TotScore[1] ) :
            string.Format("Player 1 Score: {0}", TotScore[0]);

        //animate in end level summary
        menu.OpenPanel(anim);

        //set timescale to 0
        StartCoroutine(LevelEndDelay());

        //game lost or complete
        if (!isWin || isGameComplete)
        {
            ContinueButton.gameObject.SetActive(false);
            MenuButton.Select();
            EndTxt.text = isWin ? "Congratulations You Win!" : "Game Over";
        }
        else // level beaten but game not complete
        {
            ContinueButton.gameObject.SetActive(true);
            ContinueButton.Select();
            EndTxt.text = "Level Complete";
        }
    }

    public void VSLevelOver(int winner)
    {
        bool isGameComplete = currentLevel == Levels.Length;
        //get end level stats
        for (int i = 0; i < vsPlayerMan.NumberOfPlayers; i++)
        {
            var p = vsPlayerMan.PlayersScripts[i];
            p.Lives = 3;
            VSScore[i] = (float)p.Kills*3 - p.Deaths * 2;
            Lives[i] = p.Lives;
            Deaths[i] = p.Deaths;
            Kills[i] = p.Kills;
        }
        //set stats
        KillsTxt.text = vsPlayerMan.NumberOfPlayers > 1 ? string.Format("Player 1 Kills: {0} \nPlayer 2 Kills: {1}", Kills[0], Kills[1]) :
               string.Format("Player 1 Kills: {0}", Kills[0]);
        DeathsTxt.text = vsPlayerMan.NumberOfPlayers > 1 ? string.Format("Player 1 Deaths: {0} \nPlayer 2 Deaths: {1}", Deaths[0], Deaths[1]) :
            string.Format("Player 1 Deaths: {0}", Deaths[0]);
        

        //animate in end level summary
        menu.OpenPanel(anim);

        //set timescale to 0
        StartCoroutine(LevelEndDelay());

        //game lost or complete
        if (isGameComplete)
        {
            ContinueButton.gameObject.SetActive(false);
            MenuButton.Select();
            if (VSScore[0] != VSScore[1])
            {
                EndTxt.text = VSScore[0] > VSScore[1] ? "Player 1 Wins!" : "Player 2 Wins";
            }
            else
                EndTxt.text = "Wow it's a tie";
        }
        else // level beaten but game not complete
        {
            ContinueButton.gameObject.SetActive(true);
            ContinueButton.Select();
            EndTxt.text = string.Format("Round Complete Player {0} Wins", winner + 1);
        }
    }

    IEnumerator LevelEndDelay()
    {
        yield return new WaitForEndOfFrame();
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject g in objects)
        {
            g.SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void LoadNextLevel()
    {
        
        //animate end level summary out
        if(menu != null) menu.CloseCurrent();
        StartCoroutine(LoadingScreen());
    }

    public void goMenu()
    {
        Time.timeScale = 1f;
        menu.CloseCurrent();
        Application.LoadLevel(1);
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

    IEnumerator LoadingScreen()
    {

        LoadingText.gameObject.SetActive(true);
        ProgressBar.gameObject.SetActive(true);
        bg.gameObject.SetActive(true);
        if (!isLoading)
        {
            isLoading = true;
            LoadingText.text = StaticUtilities.LevelDescription[Levels[currentLevel] - 2];
        }
        AsyncOperation async = Application.LoadLevelAsync(Levels[currentLevel++]);

        while (!async.isDone)
        {
            ProgressBar.value = async.progress;
            yield return 0;
        }

    }
}

