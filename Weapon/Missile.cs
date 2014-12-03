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


        protected override void OnEnable()
        {
            isActive = false;
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
                rigidbody2D.velocity += (Vector2)transform.up * Accel;
            }

        }

        public void ChangeDir()
        {
            isActive = !isActive;
            Invoke("ChangeDir", HomingTime);
        }
    }
}
