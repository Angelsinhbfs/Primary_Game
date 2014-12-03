using Assets.Scripts.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Shield : MonoBehaviour
    {
        private PrimaryEnums.Color color;
        private int RedHp;
        private int BlueHp;
        private int YellowHp;

        public int MaxShieldHp;

        void Start()
        {
            RedHp = MaxShieldHp;
            BlueHp = MaxShieldHp;
            YellowHp = MaxShieldHp;
            color = PrimaryEnums.Color.Yellow;
        }

        public void SetColor(PrimaryEnums.Color C)
        {
            color = C;
            switch (color)
            {
                case PrimaryEnums.Color.Yellow:
                    gameObject.SetActive(true);
                    if(YellowHp <= 0)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case PrimaryEnums.Color.Red:
                    gameObject.SetActive(true);
                    if(RedHp <= 0)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case PrimaryEnums.Color.Blue:
                    gameObject.SetActive(true);
                    if(BlueHp <= 0)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                default:
                    break;
            }
        }

        public void TakeDamage(int dmg, PrimaryEnums.Color C)
        {
            if (C != color)
            {
                switch (color)
                {
                    case PrimaryEnums.Color.Red:
                        RedHp -= dmg;
                        if (RedHp <= 0) gameObject.SetActive(false);
                        break;
                    case PrimaryEnums.Color.Yellow:
                        YellowHp -= dmg;
                        if (YellowHp <= 0) gameObject.SetActive(false);
                        break;
                    case PrimaryEnums.Color.Blue:
                        BlueHp -= dmg;
                        if (BlueHp <= 0) gameObject.SetActive(false);
                        break;
                    default:
                        break;
                }
                return;
            }

            switch (color)
            {
                case PrimaryEnums.Color.Red:
                        RedHp += dmg;
                        break;
                    case PrimaryEnums.Color.Yellow:
                        YellowHp += dmg;
                        break;
                    case PrimaryEnums.Color.Blue:
                        BlueHp += dmg;
                        break;
                    default:
                        break;
            }
            if (RedHp > MaxShieldHp) RedHp = MaxShieldHp;
            if (YellowHp > MaxShieldHp) RedHp = MaxShieldHp;
            if (BlueHp > MaxShieldHp) RedHp = MaxShieldHp;

        }

    }
}
