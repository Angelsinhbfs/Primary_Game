using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Core
{
    public class Entity : MonoBehaviour
    {
        protected int HP;
        public float MaxSpeed;
        public float Acceleration;
        public int PointValue;
        public PrimaryEnums.LockOn Locked = PrimaryEnums.LockOn.None;

        public virtual void OnCollisionEnter2D(Collision2D c)
        {

        }


        public virtual void TakeDamage(int dmg, PrimaryEnums.Color color, GameObject Owner)
        {
            //HP -= dmg;
            if (HP <=0)
            {
                Kill();
                if (Owner.GetComponent<Player.Player>() != null)
                {
                    Owner.GetComponent<Player.Player>().Score = PointValue;
                    Owner.GetComponent<Player.Player>().Kills++;
                }
            }
        }
        public void TakeDamage(int dmg, GameObject Owner)
        {
            HP -= dmg;
            if (HP <= 0)
            {
                Kill();
                
            }
        }

        public virtual void Kill()
        {
            
        }
    }
}
