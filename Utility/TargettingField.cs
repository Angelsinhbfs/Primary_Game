using Assets.Scripts.Core;
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

        void Start()
        {
            InvokeRepeating("AssessTargetList", 0.1f, AssessTime);
        }
        void Awake()
        {
            locker = isPlayerField ? gameObject.GetComponentInParent<Entity>().Locked : PrimaryEnums.LockOn.None;
        }
        void OnEnable()
        {
            Targets.Clear();
        }
        void OnTriggerEnter2D(Collider2D c)
        {
            if (Targets.Count < MaxNumber && c.GetComponent<Entity>() && c.GetComponent<Entity>().Locked == PrimaryEnums.LockOn.None)
            {
                Targets.Add(c.gameObject);
                c.GetComponent<Entity>().Locked = locker;
            }
        }

        void AssessTargetList()
        {
            foreach (GameObject g in Targets)
            {
                if (!g.activeInHierarchy)
                {
                    Targets.Remove(g);
                    break;
                }
            }
        }
        
    }
}
