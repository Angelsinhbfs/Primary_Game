
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public static class StaticUtilities
    {
        public static List<Transform> ShuffleWaypointList(List<Transform> listToShuffle)
        {
            Transform temp = null;
            int r;
            int count = listToShuffle.Count;
            for (int i = count-1; i > 0; i--)
            {
                r = Random.Range(0, i + 1);
                temp = listToShuffle[i];
                listToShuffle[i] = listToShuffle[r];
                listToShuffle[r] = temp;
            }
            return listToShuffle;
        }

        public static Quaternion XYLookRotation(Vector3 vectorA, Vector3 vectorB)
        {
            var d = vectorB - vectorA;
            var a = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg - 90;
            return Quaternion.AngleAxis(a, Vector3.forward);
        }
    }
}
