using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utility
{
    public class CreditsScroller : MonoBehaviour
    {
        private Scrollbar sBar;
        public Text textToScroll;
        private Vector2 startingPos;
        public float TimeToScrollFor;

        void Awake()
        {
            startingPos = textToScroll.rectTransform.anchoredPosition;
            sBar = gameObject.GetComponent<Scrollbar>();
            sBar.value = 1f;

        }
       
        void OnEnable()
        {
            textToScroll.rectTransform.anchoredPosition = startingPos;
            sBar.value = 1f;
        }
        void Update()
        {
            if(sBar.value > 0)
                sBar.value -= 1 / TimeToScrollFor * Time.deltaTime;
        }
    }
}
