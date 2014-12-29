using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utility;
namespace Assets.Scripts.Core
{
    public class ArcadeStarter : MonoBehaviour
    {
        public int[] AllLevelsBySceneNumber;
        private int[] thisArcadeRun = new int[6];
        public int numberOfPlayers;
        private bool arcadeSet;
        public GameObject GameManager;

        void Awake()
        {
            var temp = StaticUtilities.ShuffleIntArray(AllLevelsBySceneNumber);
            for (int i = 0; i < 6; i++)
            {
                thisArcadeRun[i] = temp[i];
            }
        }
        public void BuildArcadeRun(int n)
        {
            var g = (Instantiate(GameManager) as GameObject).GetComponent<PlayerStatManager>();
            g.Levels = thisArcadeRun;
            g.numberOfPlayers = n;
            g.LoadNextLevel();
        }
    }
       
}
