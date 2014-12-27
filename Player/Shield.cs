using Assets.Scripts.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Shield : MonoBehaviour
    {
        private SpriteRenderer rend;

        void Start()
        {
            rend = GetComponent<SpriteRenderer>();
        }
        public void SetColor(Color c)
        {
            rend.color = c;
        }
    }
}
