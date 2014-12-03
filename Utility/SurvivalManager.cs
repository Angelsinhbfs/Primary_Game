using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Enemy;

namespace Assets.Scripts.Utility
{
   
    public class SurvivalManager : MonoBehaviour
    {
        

        public GameObject[] EnemyTypes;
        public List<Transform> SpawnPoints;
        public float SongTime;
        public int NumberOfWaves;
        public float WaveOffestTime;
        public float SpawnThreshold;
        private int CurrentWave = 0;
        //public WaveList wList;
        public List<WaveList.WaveElement> Waves;
        public List<SpawnNode> LowFqSpawners;
        public List<SpawnNode> MidFqSpawners;
        public List<SpawnNode> HighFqSpawners;
        private float[] spectrum = new float[64];

        void Start()
        {
            StartCoroutine(FirstWave());
        }
        #region wave controls
        IEnumerator FirstWave()
        {
            //Debug.Log("FirstWave");
            yield return new WaitForSeconds(WaveOffestTime);
            WaveOffestTime = (SongTime - WaveOffestTime) / NumberOfWaves;
            Waves[CurrentWave].SpawnWave();
            CurrentWave++;
            if (CurrentWave < Waves.Count) yield return StartCoroutine(NewWave());

        }

        IEnumerator NewWave()
        {
            yield return new WaitForSeconds(WaveOffestTime);
            Waves[CurrentWave].SpawnWave();
            CurrentWave++;
            if (CurrentWave < Waves.Count) yield return StartCoroutine(NewWave());
        }
        #endregion

        #region audio spawns
        void Update()
        {
            AudioListener.GetOutputData(spectrum, 0);
            //Debug.Log("Highs: " + (spectrum[22] + spectrum[24]));
            if (spectrum[2] + spectrum[4] > SpawnThreshold)
            {
               // Debug.Log("spawnLow");
                SpawnEnemies(LowFqSpawners);
            }
            if (spectrum[12] + spectrum[14] > SpawnThreshold)
                SpawnEnemies(MidFqSpawners);
            if (spectrum[22] + spectrum[24] > SpawnThreshold)
                SpawnEnemies(HighFqSpawners);

        }

        void SpawnEnemies(List<SpawnNode> toSpawn)
        {
            foreach (SpawnNode s in toSpawn)
            {
                s.SpawnEnemy();
            }
        }
        #endregion
    }
}
