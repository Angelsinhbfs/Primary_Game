using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class NonScaleCamera : MonoBehaviour
    {
        public GameObject Target;
        private Vector3 workAroundTest = Vector3.zero;
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
            if (Target != null)
                workAroundTest = Target.transform.position;
            TargetPos = Vector2.Lerp(transform.position, workAroundTest, Time.deltaTime * FollowSpeed);
            gameObject.transform.position = new Vector3(TargetPos.x,TargetPos.y,-10);
        }
    }
}
