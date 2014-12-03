using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    class SwapHandler : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D c)
        {
            var g = c.GetComponent<ScreenWrap>();
            if (g != null) g.Switch();
        }

        void OnTriggerExit2D(Collider2D c)
        {
            var g = c.GetComponent<ScreenWrap>();
            if (g != null) g.isSwapping = false;
        }
    }
}
