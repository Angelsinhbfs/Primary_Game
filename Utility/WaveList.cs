using UnityEngine;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Utility
{
    public class WaveList : MonoBehaviour
    {
        #region wave element
        [System.Serializable]
        public class WaveElement
        {
            //public List<int> testList = new List<int>(1);
            public GameObject[] ElementsToSpawn = new GameObject[1];
            public void SpawnWave()
            {
                //Debug.Log("Wave element called");
                foreach (GameObject g in ElementsToSpawn)
                {
                    if (!g.activeInHierarchy)
                    {
                        g.SetActive(true);
                    }
                }
            }
        }

        public List<WaveElement> Waves = new List<WaveElement>(1);

        void AddNew()
        {
            Waves.Add(new WaveElement());
        }

        void Remove(int index)
        {
            Waves.RemoveAt(index);
        }

        #endregion
    }
}
