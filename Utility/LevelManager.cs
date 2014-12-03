using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utility
{
    public class LevelManager : MonoBehaviour
    {
        public void goPlayScene(int players)
        {
            Application.LoadLevel(players);
        }

        public void goQuit()
        {
            Application.Quit();
        }

        public void goWin()
        {

        }
    }
}
