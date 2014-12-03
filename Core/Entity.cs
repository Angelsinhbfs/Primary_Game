using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Core
{
    public class Entity : MonoBehaviour, IDamageable
    {
        public int HP;
        public float MaxSpeed;
        public float Acceleration;
        public int PointValue;
        public PrimaryEnums.LockOn Locked = PrimaryEnums.LockOn.None;

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        public virtual void Move()
        {

        }

        public virtual void OnCollisionEnter2D(Collision2D c)
        {

        }


        public void TakeDamage(int dmg)
        {
            HP -= dmg;
            if (HP <=0)
            {
                Kill();
            }
        }
        public void TakeDamage(int dmg, GameObject Owner)
        {
            HP -= dmg;
            if (HP <= 0)
            {
                Kill();
                if (Owner.GetComponent<Player.Player>() != null)
                {
                    Owner.GetComponent<Player.Player>().Score = PointValue; 
                }
            }
        }

        public virtual void Kill()
        {
            
        }
    }
}
