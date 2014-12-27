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
            if (c.gameObject.tag == "InvulnTerrain")
                Disable();
            if (c.gameObject.layer == 20) return;
            if (c.gameObject.tag != "Terrain" && c.gameObject.tag != "Bullet" && c.gameObject.tag != "InvulnTerrain")
               if(c.gameObject.GetComponent<Entity>() != null)
                   c.gameObject.GetComponent<Entity>().TakeDamage(Damage, color, Owner);

            if (c.gameObject.tag == "Terrain")
                c.GetComponent<BaseDestructibleScript>().onCollision(gameObject);
            

            if (!Piercing)
            {
                Disable();
                //Debug.Log(c.gameObject);
            }
        }

        protected virtual void Disable()
        {
            //Debug.Log("disabled");
            CancelInvoke();
            gameObject.SetActive(false);

        }

        

    }
}
