using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class Hud1Player : MonoBehaviour
    {
        public Slider Hp;
        public Text LivesObject;
        public Text ScoreObject;
        public Text Color;

        public int Lives { set { LivesObject.text = "Lives: " + value; } }
        public int Score { set { ScoreObject.text = "Score: " + value; } }
        public int HP { set { Hp.value = value; } }
        public string color { set { Color.text = value; } }

    }
}
