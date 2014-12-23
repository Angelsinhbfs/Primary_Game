using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utility;
using Assets.Scripts.Core;
using Assets.Scripts.Weapon;

namespace Assets.Scripts.Enemy
{
    public class Turret : Entity
    {
        private TargettingField tField;
        private List<GameObject> targets;
        private WeaponMount[] Mounts;
        public float ReloadTime;
        public int MaxHp;
        public float Range;

        void Awake()
        {
            HP = MaxHp;
            tField = GetComponentInChildren<TargettingField>();
            Mounts = GetComponentsInChildren<WeaponMount>();
            InvokeRepeating("TurretLogic", 0.1f, ReloadTime);
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, Range);
        }
#endif

        public virtual void TurretLogic()
        {
            targets = tField.Targets;
            foreach (var t in targets)
            {
                Debug.Log(Vector3.Distance(t.transform.position, transform.position));
                if (!t.activeInHierarchy || Vector3.Distance(t.transform.position, transform.position) > Range)
                {
                    tField.Targets.Remove(t);
                    targets.Remove(t);
                    break;
                }
            }
            foreach (var t in targets)
            {
                foreach (var m in Mounts)
                {
                    m.Fire(t);
                }
            }
        }

        public override void TakeDamage(int dmg, PrimaryEnums.Color color, GameObject Owner)
        {
            HP -= dmg;
            base.TakeDamage(dmg, color, Owner);
        }

    }
}
