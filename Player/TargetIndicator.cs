using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class TargetIndicator :MonoBehaviour
    {
        public float rotSpeed;
        public float bounceSpeed;
        public float min, max;

        void Update()
        {
            transform.Rotate(transform.forward, rotSpeed * Time.deltaTime, Space.Self);
            transform.localScale -= Vector3.one * bounceSpeed * Time.deltaTime;
            if (transform.localScale.x < min || transform.localScale.x > max)
                bounceSpeed = -bounceSpeed;

        }

        public void Attach(GameObject g)
        {
            transform.parent = g.transform;
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(true);
        }

        public void Release()
        {
            transform.parent = null;
            gameObject.SetActive(false);
        }
    }
}
