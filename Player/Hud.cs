using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class Hud : MonoBehaviour
    {
        public Slider P1Hp, P1shields;
        public Slider P2Hp, P2shields;
        public Text P1LivesObject, P2LivesObject;
        public Text P1ScoreObject, P2ScoreObject;
        public Image P1Color, P2Color;

        public int P1Lives { set { P1LivesObject.text = "Lives: " + value; } }
        public int P1Score { set { P1ScoreObject.text = "Score: " + value; } }
        public int P1HP { set { P1Hp.value = value; } }
        public int P1Shields { set { P1shields.value = value; } }
        public Color P1color { set { P1Color.color = value; } }

        public int P2Lives { set { P2LivesObject.text = value + " :Lives"; } }
        public int P2Score { set { P2ScoreObject.text = value + " :Score"; } }
        public int P2HP { set { P2Hp.value = value; } }
        public int P2Shields { set { P2shields.value = value; } }
        public Color P2color { set { P2Color.color = value; } }

        public bool enableP2Hud 
        {
            set
            {
                P2Hp.gameObject.SetActive(value);
                foreach (Image i in P2Hp.GetComponentsInChildren<Image>())
                {
                    i.enabled = value;
                } 
                P2LivesObject.gameObject.SetActive(value);
                P2ScoreObject.gameObject.SetActive(value);
                P2shields.gameObject.SetActive(value);
            }
        }

    }
}
