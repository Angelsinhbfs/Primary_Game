using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Core;

namespace Assets.Scripts.Weapon
{
    public class Lazer :Projectile
    {
        private LineRenderer Renderer;
        private BoxCollider2D Area;
        private Vector3 EndPoint;
        private Vector2 Forward;
        private Vector3 MidPoint;
        private float CumulativeDmg;

        void Awake()
        {
            Renderer = gameObject.GetComponent<LineRenderer>();
            Area = gameObject.GetComponent<BoxCollider2D>();
            Stationary = true;
            Piercing = true;
           
        }

        protected override void OnEnable()
        {
            EndPoint = transform.position;
            rigidbody2D.velocity = Vector2.zero;
            Renderer.SetPosition(0, Vector2.zero);
            Renderer.SetPosition(1, EndPoint);
            UpdtateRendererPos();
            UpdateColliderPos();
            Renderer.enabled = true;
            CumulativeDmg = 0f;
            base.OnEnable();

        }

        public override void DoLogic()
        {
            UpdtateRendererPos();
            UpdateColliderPos();
        }

        private void UpdateColliderPos()
        {
            MidPoint = new Vector3((transform.position.x + EndPoint.x) * 0.5f, (transform.position.y + EndPoint.y) * 0.5f);
            Area.center = transform.InverseTransformPoint(MidPoint);
            Area.size = new Vector2(0.5f,Vector3.Distance(transform.position, EndPoint));
        }

        private void UpdtateRendererPos()
        {
            EndPoint += transform.up * Speed * Time.deltaTime;
            Renderer.SetPosition(1, transform.InverseTransformPoint(EndPoint));
        }

        void OnTriggerStay2D(Collider2D c)
        {
            if (c.gameObject == Owner) return;
            if (c.gameObject.tag == "Terrain") return;
            CumulativeDmg += Damage * Time.deltaTime * 2;
        }

        void OnTriggerExit2D(Collider2D c)
        {
            if (c.gameObject == Owner) return;
            if (c.gameObject.tag == "Terrain") return;
            //Debug.Log(c.name);
            //Debug.Log(c);
            c.GetComponent<Entity>().TakeDamage((int)CumulativeDmg, Owner);
            CumulativeDmg = 0;
        }
        protected override void Disable()
        {
            Renderer.enabled = false;
            base.Disable();
        }
    }
}
