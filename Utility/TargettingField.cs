using Assets.Scripts.Core;
using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class TargettingField : MonoBehaviour
    {
        public List<GameObject> Targets;
        public float AssessTime;
        public int MaxNumber;
        public bool isPlayerField;
        public PrimaryEnums.LockOn locker;
        private ObjectPooler indicators;

        void Start()
        {
            InvokeRepeating("AssessTargetList", 0.1f, AssessTime);
            if (isPlayerField) indicators = GetComponent<ObjectPooler>();
        }
        void Awake()
        {
            locker = isPlayerField ? gameObject.GetComponentInParent<Entity>().Locked : PrimaryEnums.LockOn.None;
        }
        void OnEnable()
        {
            Targets.Clear();
        }

        void OnDisable()
        {
            
            if (!isPlayerField) return;
            Debug.Log("on disable ran");
            foreach (GameObject g in indicators.Pool)
            {
                var i = g.GetComponentInChildren<TargetIndicator>();
                if (i != null) i.Release();
            }
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            if (Targets.Count < MaxNumber && c.GetComponent<Entity>() && c.GetComponent<Entity>().Locked == PrimaryEnums.LockOn.None)
            {
                Targets.Add(c.gameObject);
                c.GetComponent<Entity>().Locked = locker;
                if(isPlayerField)
                {
                    var i = indicators.GetInstance();
                    i.GetComponent<TargetIndicator>().Attach(c.gameObject);
                }
            }
        }

        void AssessTargetList()
        {
            foreach (GameObject g in Targets)
            {
                if (!g.activeInHierarchy)
                {
                    var i = g.GetComponentInChildren<TargetIndicator>();
                    if (i != null) i.Release();
                    Targets.Remove(g);
                    break;
                }
            }
        }
        
    }
}
