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
    public class VSManager : MonoBehaviour
    {

        #region Variables
        //timer related
        public Text TimeRem;
        private bool paused;
        private float elapsedTime;
        private float SongTime = 0f;
        public float songTime { get { return SongTime; } }

        //multiple songs
        public List<AudioClip> songs;
        private int currentSong = 0;


        private bool levelComplete;


        #endregion
        void Start()
        {
            songs = StaticUtilities.ShuffleList(songs);
            audio.clip = songs[0];
            audio.Play();
            StartCoroutine(CountDown());
        }

        void Update()
        {
            if (paused) return;
            if (!audio.isPlaying && currentSong < songs.Count - 1)
            {
                currentSong++;
                audio.clip = songs[currentSong];
                audio.Play();
            }

        }

        IEnumerator CountDown()
        {
            yield return new WaitForSeconds(0.05f);
            if (!paused)
            {
                elapsedTime = Time.timeSinceLevelLoad;
                TimeRem.text = elapsedTime.ToString("F");
                StartCoroutine(CountDown());

            }
        }

        public void OnPause()
        {
            paused = true;
        }
    }
}
