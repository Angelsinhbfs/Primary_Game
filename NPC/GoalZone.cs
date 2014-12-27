using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Utility;
using System.Collections;

namespace Assets.Scripts.NPC
{
    public class GoalZone : MonoBehaviour
    {
        private ProtectManager protectMan;
        private Text captureTimer;
        private bool isOccupied;
        public bool mustStay;
        public string TagToRespondTo;
        private bool paused;
        private float elapsedTime;
        public float CaptureTime;

        void Awake()
        {
            protectMan = GameObject.FindGameObjectWithTag("SceneManagers").GetComponent<ProtectManager>();
            captureTimer = GetComponentInChildren<Text>();
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            //Debug.Log("entered goal zone");
            if (isOccupied) return;
            if (c.tag != TagToRespondTo) return;
            //Debug.Log("accepted tag");
            isOccupied = true;
            protectMan.SendMessage("inEndZone");
            StartCoroutine(CountDown());

        }

        void OnTriggerExit2D(Collider2D c)
        {
            if (!mustStay) return;
            if (c.tag != TagToRespondTo) return;
            StopCoroutine(CountDown());
            elapsedTime = 0;
        }

        IEnumerator CountDown()
        {
            yield return new WaitForSeconds(0.05f);
            if (!paused)
            {
                elapsedTime += 0.05f;
                captureTimer.text = (CaptureTime - elapsedTime).ToString("F");
                if (CaptureTime - elapsedTime <= 0)
                {
                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStatManager>().LevelOver(true, false);
                }
                else
                    yield return StartCoroutine(CountDown());
            }


        }

        public void OnPause()
        {
            paused = true;
        }

    }
}
