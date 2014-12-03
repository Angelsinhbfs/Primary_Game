using Assets.Scripts.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class ScreenWrap : MonoBehaviour
    {
        public  bool isSwapping;
        public void Switch()
        {
            isSwapping = true;
            //Debug.Log("off camera");
            Camera c = Camera.main;
            Vector3 vPos = c.WorldToViewportPoint(transform.position);
            Vector3 nPos = transform.root.position;

            if (vPos.x > 1 || vPos.x < 0)
                nPos.x = vPos.x > 1 ? -(nPos.x - 3) : -(nPos.x +3);//-nPos.x;
            if (vPos.y > 1 || vPos.y < 0)
                nPos.y = vPos.y > 1 ? -(nPos.y - 3) : -(nPos.y + 3);//-nPos.y;

            transform.root.position = nPos;

            //if(isControlled)
            //{
            //    transform.root.gameObject.GetComponent<Entity>().enabled = false;
            //}
        }

        //void OnBecameVisible()
        //{
        //    if (isControlled) transform.root.gameObject.GetComponent<Entity>().enabled = true;
        //}
    }
}
