using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;

namespace Assets.Scripts.Weapon
{
    
    public class WeaponMount : MonoBehaviour
    {
        public ObjectPooler Bullets;
        private GameObject Owner;
        public bool isPlayerOwned;
        public PrimaryEnums.FireDir fireDir;
        public Vector2 Offset;
        

        // Use this for initialization
        void Awake()
        {
            Bullets = gameObject.GetComponent<ObjectPooler>();
            Owner = gameObject.transform.root.gameObject;
            Bullets.optionalOwner = Owner;    
        }

        void OnDrawGizmos()
        {
            Debug.DrawLine(transform.position, transform.position + new Vector3(Offset.x,Offset.y));
        }

        public void Fire()
        {
            //Debug.Log("trying to get bullet");
            GameObject b = Bullets.GetInstance();
            if (b == null) return;
            //Debug.Log(b);
            b.transform.rotation = transform.rotation;
            b.transform.position = transform.TransformPoint(Offset.x, Offset.y,0f);
            switch (fireDir)
            {
                case PrimaryEnums.FireDir.Front:
                    break;
                case PrimaryEnums.FireDir.Left:
                    b.SetActive(true);
                    b.rigidbody2D.velocity = (Vector2)transform.right * -b.GetComponent<Projectile>().Speed;
                    b.rigidbody2D.velocity += Owner.rigidbody2D.velocity;
                    break;
                case PrimaryEnums.FireDir.Right:
                    b.SetActive(true);
                    b.rigidbody2D.velocity = (Vector2)transform.right * b.GetComponent<Projectile>().Speed;
                    b.rigidbody2D.velocity += Owner.rigidbody2D.velocity;
                    break;
                default:
                    break;
            }
            b.SetActive(true);
            
        }

        public void Fire(GameObject Target)
        {
            //Debug.Log("trying to get bullet");
            GameObject b = Bullets.GetInstance();
            if (b == null) return;
            //Debug.Log(b);
            b.transform.rotation = transform.rotation;
            b.transform.position = transform.TransformPoint(Offset.x, Offset.y, 0f);
            switch (fireDir)
            {
                case PrimaryEnums.FireDir.Front:
                    break;
                case PrimaryEnums.FireDir.Left:
                    b.SetActive(true);
                    b.rigidbody2D.velocity = (Vector2)transform.right * -b.GetComponent<Projectile>().Speed;
                    break;
                case PrimaryEnums.FireDir.Right:
                    b.SetActive(true);
                    b.rigidbody2D.velocity = (Vector2)transform.right * b.GetComponent<Projectile>().Speed;
                    break;
                default:
                    break;
            }
            b.GetComponent<Projectile>().Target = Target;
            b.SetActive(true);

        }

    }
}
