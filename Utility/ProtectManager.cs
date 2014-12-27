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
    public class ProtectManager : MonoBehaviour
    {
        #region Variables
        //timer related
        public Text TimeRem;
        private bool paused;
        private float elapsedTime;
        private float SongTime;
        public float songTime { get { return SongTime; } }

        //enemy management
        public List<SpawnNode> Outer;
        public List<SpawnNode> Inner;
        public List<GameObject> OuterPatrolP;
        public List<GameObject> InnerPatrolP;
        public List<WaveList.WaveElement> Waves;
        private int currentWave = 0;



        #endregion

        void Start()
        {
            SongTime = audio.clip.length;
            StartCoroutine(CountDown());
        }

        public void DoorDown()
        {
            if (Waves.Count == 0) return;
            if (currentWave == Waves.Count) return;
            Waves[currentWave].SpawnWave();
            currentWave++;
        }

        public void inEndZone()
        {
            Debug.Log("started goal waves");
            foreach (var g in Inner)
            {
                g.gameObject.SetActive(true);
            }
            foreach (var g in Outer)
            {
                g.gameObject.SetActive(true);
            }
            foreach (var p in OuterPatrolP)
            {
                p.SetActive(false);
            }
            foreach (var p in InnerPatrolP)
            {
                p.SetActive(true);
            }
            StartCoroutine(SpawnReinforcements());
        }

        IEnumerator SpawnReinforcements()
        {
            switch (UnityEngine.Random.Range(0, 2))
            {
                case 0:
                    foreach (var s in Inner)
                    {
                        s.SpawnEnemy();
                    }
                    break;
                case 1:
                    foreach (var s in Outer)
                    {
                        s.SpawnEnemy();
                    }
                    break;
            }
            yield return StartCoroutine(StaticUtilities.Wait(5f));
            if (elapsedTime < SongTime)
                yield return StartCoroutine(SpawnReinforcements());
        }

        IEnumerator CountDown()
        {
            yield return new WaitForSeconds(0.05f);
            if (!paused)
            {
                elapsedTime += 0.05f;
                TimeRem.text = (SongTime - elapsedTime).ToString("F");
                if (SongTime - elapsedTime <= 0)
                {
                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStatManager>().LevelOver(false, true);
                }
                else
                    yield return StartCoroutine(CountDown());
            }
            else
                yield return StartCoroutine(CountDown());

        }

        public void OnPause()
        {
            paused = true;
        }


    }
}
