using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Core;
using Assets.Scripts.Utility;
using Assets.Scripts.Enemy;

namespace Assets.Scripts.Utility
{
    public class SkirmishManager : MonoBehaviour
    {

        #region Variables
        //timer related
        public Text TimeRem;
        private bool paused;
        private float elapsedTime;
        private float SongTime = 0f;
        public float songTime { get { return SongTime; } }

        //multiple songs
        public AudioClip[] songs;
        private int currentSong = 0;

        //enemy management
        public List<SpawnNode> Spawners = new List<SpawnNode>();
        public float timeBetweenSpawns;

        private bool levelComplete;


        #endregion

        void Awake()
        {
            var s = GameObject.FindGameObjectsWithTag("EnemySpawn");
            foreach (GameObject sp in s)
            {
                Spawners.Add(sp.GetComponent<SpawnNode>());
            }
        }
        

        void Start()
        {
            
            foreach (AudioClip a in songs)
            {
                SongTime += a.length;
            }
            audio.clip = songs[0];
            audio.Play();
            StartCoroutine(CountDown());
            InvokeRepeating("SpawnWave", 0.1f, timeBetweenSpawns);
        }

        void Update()
        {
            if (paused) return;
            if(!audio.isPlaying && currentSong < songs.Length -1)
            {
                currentSong++;
                audio.clip = songs[currentSong];
                audio.Play();
            }

            foreach (var sp in Spawners)
            {
                if(!sp.gameObject.activeInHierarchy)
                {
                    Spawners.Remove(sp);
                    break;
                }
            }

            if (Spawners.Count == 0 && !levelComplete)
            {
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStatManager>().LevelOver(true);
                levelComplete = true;
            }
        }

        public void SpawnWave()
        {
            foreach (var s in Spawners)
            {
                if(s.gameObject.activeInHierarchy)
                    s.SpawnEnemy();
            }
        }
        IEnumerator CountDown()
        {
            yield return new WaitForSeconds(0.05f);
            if (!paused)
            {
                elapsedTime = Time.timeSinceLevelLoad;
                TimeRem.text = (SongTime - elapsedTime).ToString("F");
                if (SongTime - elapsedTime <= 0 && !levelComplete)
                {
                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStatManager>().LevelOver(false);
                    levelComplete = true;
                }
                else
                    StartCoroutine(CountDown());

            }
        }

        public void OnPause()
        {
            paused = true;
        }
    }
}
