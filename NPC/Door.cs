using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utility;


namespace Assets.Scripts.NPC
{
    public class Door : MonoBehaviour
    {
        private ProtectManager protectMan;

        void Awake()
        {
            protectMan = GameObject.FindGameObjectWithTag("SceneManagers").GetComponent<ProtectManager>();
        }
        void OnTriggerEnter2D(Collider2D c)
        {
            if (c.tag != "Player") return;

            protectMan.SendMessage("DoorDown");
            //Debug.Log("Door triggered");
            //add explosion particle effect call here
            //Destroy(transform.root.gameObject);
            transform.root.gameObject.SetActive(false);
        }
    }
}
