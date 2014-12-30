using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class NonScaleCamera : MonoBehaviour
    {
        public GameObject[] Targets;
        Vector2 TargetPos;
        public float FollowSpeed;
        public float baseAspect = 1920 / 1080;

        void Start()
        {
            var curAspect = Screen.width / Screen.height;
            camera.orthographicSize = Screen.height / 2 /8;
        }
        void Update()
        {
            //Debug.Log("target according to camera is: " + Target);
            TargetPos = Targets[1] != null ? Vector2.Lerp(Targets[0].transform.position,Targets[1].transform.position,0.5f) : (Vector2)Targets[0].transform.position;
            //TargetPos = Targets.Length == 2 ? Vector2.Lerp(Targets[0].transform.position, Targets[1].transform.position, 0.5f) : (Vector2)Targets[0].transform.position;

            TargetPos = Vector2.Lerp(transform.position, TargetPos, Time.deltaTime * FollowSpeed);
            gameObject.transform.position = new Vector3(TargetPos.x,TargetPos.y,-10);
        }
    }
}
