using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject CameraPrefab;
        public void BuildCamera(float AspectRatio, GameObject[] Targets,float FollowSpeed)
        {
            var c = Instantiate(CameraPrefab) as GameObject;
            c.SetActive(false);
            var s = c.AddComponent<NonScaleCamera>();

            s.Targets = Targets;
            //Debug.Log("Target passed is: " + Target);
            //Debug.Log("Target recieved is: " + s.Target);
            s.baseAspect = AspectRatio;
            s.FollowSpeed = FollowSpeed;
            c.SetActive(true);

        }
        //public void BuildCamera(float AspectRatio, GameObject Target1, GameObject Target2,float FollwSpeed, bool isScaled)
        //{
        //    if (isScaled)
        //    {
        //        //scaling and moving camera
        //    }
        //    else
        //    {
        //        //split screen 2 camera system
        //    }
        //}

    }
}
