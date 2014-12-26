using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class BGStarsDimsSet : MonoBehaviour
    {
        int h, v;

        void Start()
        {
            h = Screen.width;
            v = Screen.height;
            renderer.material.SetVector("_Dims", new Vector4(Screen.width, Screen.height, 0, 0));
        }
        //for debug purposes only
        void Update()
        {
            h += (int)Input.GetAxis("POVHatHP1") * 10;
            v += (int)Input.GetAxis("POVHatVP1") * 10;
            h += (int)Input.GetAxis("Horizontal") * 10;
            v += (int)Input.GetAxis("Vertical") * 10;
            renderer.material.SetVector("_Dims", new Vector4(h, v, 0, 0));
        }
    }
}
