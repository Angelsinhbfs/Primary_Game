using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using Assets.Scripts.Player;
using Assets.Scripts.Weapon;

namespace Assets.Scripts.Enemy
{
    public class BeamEnemy : Enemy
    {
        //unit specific stats
        private WeaponMount[] Weapons;
        public float leadTime;
        public float reloadTime;

        void Awake()
        {
            Weapons = GetComponentsInChildren<WeaponMount>();
        }

        protected override void Attack()
        {
            rigidbody2D.velocity = Vector2.zero;
            StartCoroutine(ReadyFire());
        }
        IEnumerator ReadyFire()
        {
            //Debug.Log("readying to fire");
            AssessTarget();
            transform.up = aim();
            fire();
            yield return new WaitForSeconds(reloadTime);
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
