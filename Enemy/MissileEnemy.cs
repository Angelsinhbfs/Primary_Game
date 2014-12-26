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
    public class MissileEnemy : Enemy
    {
        //unit specific stats
        private WeaponMount[] Weapons;
        public float leadTime;
        public float reloadTime;
        public float turnSpeed;

        void Start()
        {
            Weapons = GetComponentsInChildren<WeaponMount>();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAttacking) offPathMove();
            else rigidbody2D.velocity = Vector2.zero;
        }

        protected override void Attack()
        {
            StartCoroutine(Strafe());
        }
        IEnumerator Strafe()
        {
            //Debug.Log("readying to fire");
            AssessTarget();
            if (Target == null) yield return 0;
            transform.up = aim();
            Speed = MaxSpeed * 1.15f;
            float t = Vector2.Distance(Target.transform.position, transform.position) * 0.5f;
            t /= getDV();
            yield return StartCoroutine(StaticUtilities.Wait(t));
            AssessTarget();
            if (Target == null) yield return 0;
            fire();
            disengage();
            yield return StartCoroutine(StaticUtilities.Wait(t * 3));
            isAttacking = false;

        }

        private void disengage()
        {
            var toAngle = Mathf.Atan2(Target.transform.position.y - transform.position.y, Target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;// + (int)UnityEngine.Random.Range(0,1) * 180;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(toAngle, Vector3.forward),  turnSpeed);
            //Target = null;
        }

        Vector3 aim()
        {
            //Debug.Log("aiming");
            var t = Target.transform.position;
            t = t - transform.position;
            return t;
        }
        float getDV()
        {
            var tV = Vector2.Dot(Target.rigidbody2D.velocity, Vector3.Normalize(transform.position - Target.transform.position));
            return tV + Speed;
        }

        void fire()
        {
            //Debug.Log("Fireing");
            foreach (WeaponMount m in Weapons)
            {
                m.Fire(Target);
            }
        }
    }
}
