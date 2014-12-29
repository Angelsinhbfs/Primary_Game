
using System.Collections;
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

        public static int[] ShuffleIntArray(int[] input)
        {
            int temp = 0;
            int r;
            int count = input.Length;
            for (int i = count - 1; i > 0; i--)
            {
                r = Random.Range(0, i + 1);
                temp = input[i];
                input[i] = input[r];
                input[r] = temp;
            }
            return input;
        }

        public static Quaternion XYLookRotation(Vector3 vectorA, Vector3 vectorB)
        {
            var d = vectorB - vectorA;
            var a = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg - 90;
            return Quaternion.AngleAxis(a, Vector3.forward);
        }

        public static IEnumerator Wait(float duration)
        {
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
                yield return 0;
        }
    }
}
