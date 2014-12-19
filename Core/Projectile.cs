using Assets.Tools.Ferr._2DTerrain.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Projectile : MonoBehaviour
    {
        public int Damage;
        public int Speed;
        public int Life;

        public bool Stationary;
        public bool Piercing;
        public GameObject Owner;
        public GameObject Target;
        public PrimaryEnums.Color color;

        protected virtual void OnEnable()
        {
            CancelInvoke();
            Invoke("Disable", Life);
            if (!Stationary)
            {
                rigidbody2D.velocity += new Vector2(transform.up.x, transform.up.y) * Speed; 
            }
        }

        void Update()
        {
                DoLogic();
        }

        public virtual void DoLogic()
        {
            
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            //Debug.Log(c.name);
            //Debug.Log(c);
            if (c.gameObject.transform.root.gameObject == Owner) return;

            if (c.gameObject.tag != "Terrain" && c.gameObject.tag != "Bullet")
               c.gameObject.GetComponent<Entity>().TakeDamage(Damage,color, Owner);

            if (c.gameObject.tag == "Terrain")
                c.GetComponent<BaseDestructibleScript>().onCollision(gameObject);

            if (!Piercing)
            {
                Disable();
            }
        }

        protected virtual void Disable()
        {
            CancelInvoke();
            gameObject.SetActive(false);

        }

        

    }
}
