using Assets.Scripts.Utility;
using Assets.Scripts.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class BulletEnemy : Enemy
    {
        //unit specific stats
        private WeaponMount[] Weapons;
        public float leadTime;
        public float reloadTime;
        public float minRange;

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, minRange);
        }
#endif
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Target == null) return;
            if (Vector2.Distance(Target.transform.position, transform.position) < minRange)
            {

                agent.Stop();
            }
        }
        void Start()
        {
            Weapons = GetComponentsInChildren<WeaponMount>();
        }

        protected override void Attack()
        {

            StartCoroutine(ReadyFire());
        }
        IEnumerator ReadyFire()
        {
            //Debug.Log("readying to fire");
            AssessTarget();
            transform.up = aim();
            fire();
            yield return StartCoroutine(StaticUtilities.Wait(reloadTime));
            if (Target != null && Vector3.Distance(transform.position, Target.transform.position) <= maxAttackDistance)
            {
                StartCoroutine(ReadyFire());
            }
            else
            {
                isAttacking = false;
            }

        }

        Vector3 aim()
        {
            //Debug.Log("aiming");
            if (Target == null) return transform.position;
            var t = Target.transform.position;
            var v = Target.rigidbody2D.velocity;
            t = new Vector3(t.x + v.x * leadTime, t.y + v.y * leadTime);
            t = t - transform.position;
            return t;

        }

        void fire()
        {
            //Debug.Log("Fireing");
            foreach (WeaponMount m in Weapons)
            {
                m.Fire();
            }
        }
    }
}
