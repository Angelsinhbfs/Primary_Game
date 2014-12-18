using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;

namespace Assets.Scripts.Enemy
{
    public class SpawnNode : Entity
    {
        //public GameObject EnemyType;
        public ObjectPooler EnemyPool;
        public int maxHp;
        //public bool canInc;
        //public int StartingEnemies;


            // Use this for initialization
        
        public void SpawnEnemy()
        {
            if (!gameObject.activeInHierarchy) return;
            var e = EnemyPool.GetInstance();
            if (e != null)
            {
                e.transform.position = transform.position;
                e.SetActive(true);
            }
        }

        void OnEnable()
        {
            HP = maxHp;
        }
        public override void OnCollisionEnter2D(Collision2D c)
        {
            //c.gameObject.GetComponent<Entity>().TakeDamage(25);
            //TakeDamage(25);
        }

        public override void TakeDamage(int dmg, PrimaryEnums.Color color, GameObject Owner)
        {
            HP -= dmg;
            base.TakeDamage(dmg, color, Owner);
        }

        public override void Kill()
        {
            gameObject.SetActive(false);
        }
    }
}
