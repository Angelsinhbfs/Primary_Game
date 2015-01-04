using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utility;
using UnityEngine.UI;
namespace Assets.Scripts.Core
{
    public class ArcadeStarter : MonoBehaviour
    {
        public List<int> AllLevelsBySceneNumber;
        public List<int> VersusLevels;
        public Text[] counters;
        private int[] thisArcadeRun;
        private int[] thisVersusRun;
        private int numberOfPlayers;
        public bool isTwoPlayer { get; set; }
        private bool _isArcade;
        public bool isArcade 
        { 
            get 
            { 
                return _isArcade; 
            } 
            set 
            { 
                _isArcade = value; 
                numberOfLevels = isArcade ? Mathf.Clamp(numberOfLevels, 1, 6) : Mathf.Clamp(numberOfLevels, 1, 3);
                counters[0].text = numberOfLevels.ToString();
                counters[1].text = numberOfLevels.ToString();
            } 
        }
        private int numberOfLevels = 3;
        private bool arcadeSet;
        public GameObject GameManager;



        public void BuildRun()
        {
            var g = (Instantiate(GameManager) as GameObject).GetComponent<PlayerStatManager>();
            if (isArcade)
            {
                thisArcadeRun = new int[numberOfLevels];
                var temp = StaticUtilities.ShuffleList(AllLevelsBySceneNumber);
                for (int i = 0; i < numberOfLevels; i++)
                {
                    thisArcadeRun[i] = temp[i];
                }
                g.Levels = thisArcadeRun;
                g.numberOfPlayers = isTwoPlayer ? 2 : 1;
                g.LoadNextLevel();
            }
            else
            {
                numberOfLevels = Mathf.Clamp(numberOfLevels, 1, 3);
                thisVersusRun = new int[numberOfLevels];
                var temp = StaticUtilities.ShuffleList(VersusLevels);
                for (int i = 0; i < numberOfLevels; i++)
                {
                    thisVersusRun[i] = temp[i];
                }
                g.Levels = thisVersusRun;
                g.numberOfPlayers = 2;
                g.LoadNextLevel();
            }
        }


        public void SetNumberOfLevels(int positive)
        {
            numberOfLevels += 1 * positive;
            numberOfLevels = isArcade ? Mathf.Clamp(numberOfLevels, 1, 6) : Mathf.Clamp(numberOfLevels, 1, 3);
            counters[0].text = numberOfLevels.ToString();
            counters[1].text = numberOfLevels.ToString();

        }


    }
       
}
