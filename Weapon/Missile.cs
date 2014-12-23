using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Core;

namespace Assets.Scripts.Weapon
{
    public class Missile : Projectile
    {
        public float FireDelay;
        public int HomingTime;
        public int Accel;
        private bool isActive = false;
        private float adjSpeed;


        protected override void OnEnable()
        {
            isActive = false;
            adjSpeed = Speed;
            base.OnEnable();
            Invoke("ChangeDir", FireDelay);
        }

        public override void DoLogic()
        {
            if (isActive)
            {
                if (Target != null && Target.activeInHierarchy)
                {
                    Vector3 d = transform.position - Target.transform.position;
                    d.Normalize();

                    transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg + 90, Vector3.forward);
                }
                rigidbody2D.velocity += (Vector2)Vector3.Normalize(transform.up) * Accel;
                adjSpeed++;
                rigidbody2D.velocity = Vector3.Normalize(new Vector3(rigidbody2D.velocity.x, rigidbody2D.velocity.y)) * adjSpeed;
            }

        }

        public void ChangeDir()
        {
            isActive = !isActive;
            Invoke("ChangeDir", HomingTime);
        }
    }
}
