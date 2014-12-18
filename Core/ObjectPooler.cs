using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
 
    public class ObjectPooler : MonoBehaviour
    {
        public GameObject PooledObject;
        public List<GameObject> Pool;
        public int StartingCount;
        public int MaxCount = 0;
        public bool canGenerateNewObjects;
        public GameObject optionalOwner = null;

        void Start()
        {
            for (int i = 0; i < StartingCount; i++)
            {
                GameObject o = (GameObject)Instantiate(PooledObject);
                o.SetActive(false);
                if (o.GetComponent<Projectile>() != null && optionalOwner != null)
                    o.GetComponent<Projectile>().Owner = optionalOwner;
                Pool.Add(o);
            }
        }

        public GameObject GetInstance()
        {
            foreach (GameObject g in Pool)
            {
                if (!g.activeInHierarchy)
                {
                    //Debug.Log("object found");
                    return g;
                }
            }
            if (canGenerateNewObjects)
            {
                if (MaxCount == 0 || Pool.Count - 1 < MaxCount)
                {
                    GameObject n = (GameObject)Instantiate(PooledObject);
                    Pool.Add(n);
                    n.SetActive(false);
                    return n; 
                }
            }
            return null;
        }
    }
}
