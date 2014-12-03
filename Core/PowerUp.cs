using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    class PowerUp : MonoBehaviour
    {
        public Vector2 Velocity;
        public int Life;

        void OnEnable()
        {
            Invoke("Kill", Life);
            rigidbody2D.velocity = Velocity;
        }

        void OnCollisionEnter2D(Collision2D c)
        {
            if (c.gameObject.tag != "Terrain")
            {
                ApplyEffect(c.gameObject);
                Kill();
            }
            else
                rigidbody2D.velocity = -rigidbody2D.velocity;
        }

        private void Kill()
        {
            Destroy(gameObject);
        }

        public virtual void ApplyEffect(GameObject Player)
        {
            throw new NotImplementedException();
        }
    }
}
