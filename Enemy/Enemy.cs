using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using Assets.Scripts.Player;

namespace Assets.Scripts.Enemy
{
    public class Enemy : Entity
    {
        public PrimaryEnums.AiState State = PrimaryEnums.AiState.Wander;
        public PrimaryEnums.Color color;
        public int MaxHp;
        public float Speed;

        public GameObject Target;
        public Vector3 moveToPos;

        private bool paused;


        #region pathfinding
        public List<Transform> PatrolPoints;
        public TargettingField tField;
        private bool isPointsCollected = false;
        protected PolyNavAgent agent;

        //The calculated path
        private int currentPatrolPoint;
        #endregion

        #region ai behavior
        //attack state triggers
        public float minAttackDistance;
        public float maxAttackDistance;
        public bool isAttacking;
        public bool isStationaryAttacker;
        public Vector3 TargetTrackingOffset;
        private Vector3 TargetPos, oldTargetPos;
        protected bool dirtyPath = false;
        #endregion

        protected void BuildPatrolPoints()
        {
            var t = GameObject.FindGameObjectsWithTag("PatrolPoint");
            //Debug.Log(t.Length);
            for (int i = 0; i < t.Length; i++)
            {
                PatrolPoints.Add(t[i].transform);
            }
            isPointsCollected = true;
        }

        void Start()
        {
            agent = GetComponent<PolyNavAgent>();

        }
        void OnEnable()
        {
            if (agent == null) agent = GetComponent<PolyNavAgent>();
            Target = null;
            isAttacking = false;
            rigidbody2D.velocity = Vector2.zero;
            HP = MaxHp;
            //InvokeRepeating("AssessTarget", 0.1f, 0.5f);
            BuildPatrolPoints();
            //Debug.Log(PatrolPoints);
            PatrolPoints = StaticUtilities.ShuffleWaypointList(PatrolPoints);

            agent.SetDestination(PatrolPoints[0].position);
        }

        public virtual void FixedUpdate()
        {
            if(!paused)
                doLogic();
        }
        protected void doLogic()
        {
            if (!isPointsCollected)
                BuildPatrolPoints();
            AssessTarget();
            if (Target != null)
            {
                if (!Target.activeInHierarchy) State = PrimaryEnums.AiState.Wander;
                if (Vector2.Distance(transform.position, Target.transform.position) <= minAttackDistance)
                    State = PrimaryEnums.AiState.Attack;
                else if (State == PrimaryEnums.AiState.Attack && Vector2.Distance(transform.position, Target.transform.position) >= maxAttackDistance)
                    State = PrimaryEnums.AiState.Follow;
                else if (State != PrimaryEnums.AiState.Attack && Vector2.Distance(transform.position, Target.transform.position) > minAttackDistance)
                    State = PrimaryEnums.AiState.Follow;
            }
            else
                State = PrimaryEnums.AiState.Wander;

            switch (State)
            {
                case PrimaryEnums.AiState.Wander:
                    Speed =(int)( MaxSpeed * 0.5f);
                    agent.maxSpeed = Speed;
                    TargetPos = PatrolPoints[currentPatrolPoint].position;
                    CheckPath(TargetPos);
                    break;
                case PrimaryEnums.AiState.Follow:
                    Speed = (int)MaxSpeed;
                    agent.maxSpeed = Speed;
                    TargetPos = Target.transform.TransformPoint(TargetTrackingOffset);
                    CheckPath(TargetPos);
                    break;
                case PrimaryEnums.AiState.Attack:
                    if (!isAttacking)
                    {
                        Attack();
                        isAttacking = true;
                    }
                    if (!isStationaryAttacker)
                    {
                        Speed = (int)MaxSpeed;
                        agent.maxSpeed = Speed;
                        if (Target == null) return;
                        TargetPos = Target.transform.TransformPoint(TargetTrackingOffset);
                        CheckPath(TargetPos);
                    }
                    else
                        agent.Stop();

                    break;
                default:
                    break;
            }
        }

        private void Rotate()
        {
            transform.rotation = StaticUtilities.XYLookRotation(transform.position, moveToPos);
        }

        protected virtual void Attack()
        {
            throw new System.NotImplementedException();
        }

        private void CheckPath(Vector3 point)
        {
             //builds patrol path and wanders
            if (!isPointsCollected) return;
            if (!agent.pathPending && !agent.hasPath)//clear prime goal so it trys to calculate path again
                agent.primeGoal = Vector2.zero;
            if (point != oldTargetPos || agent.activePath.Count == 0)
            {
                agent.SetDestination(point);
                oldTargetPos = point;
            }
            else if (State == PrimaryEnums.AiState.Wander && agent.remainingDistance < 1)
            {
                currentPatrolPoint++;
                if (currentPatrolPoint == PatrolPoints.Count) currentPatrolPoint = 0;
            }
        }

        protected void offPathMove()
        {
            rigidbody2D.velocity += (Vector2)transform.up * Speed;
            rigidbody2D.velocity = Vector3.Normalize(new Vector3(rigidbody2D.velocity.x, rigidbody2D.velocity.y)) * Speed;
        }

        public override void TakeDamage(int dmg, PrimaryEnums.Color color, GameObject Owner)
        {
            HP -= dmg;
            if (Target == null) Target = Owner;
            base.TakeDamage(dmg, color, Owner);
        }

        public void OnDestinationReached()
        {
            if (State == PrimaryEnums.AiState.Wander)
            {
                currentPatrolPoint++;
                if (currentPatrolPoint == PatrolPoints.Count) currentPatrolPoint = 0;
                if (Vector3.Distance(transform.position, PatrolPoints[currentPatrolPoint].transform.position) < 1f) currentPatrolPoint++;
                if (currentPatrolPoint == PatrolPoints.Count) currentPatrolPoint = 0;
                agent.SetDestination(PatrolPoints[currentPatrolPoint].transform.position);
            }
            //else
            //    AssessTarget();
            //if(Target == null)
            //{
            //    currentPatrolPoint++;
            //    if (currentPatrolPoint == PatrolPoints.Count) currentPatrolPoint = 0;
            //    agent.SetDestination(PatrolPoints[currentPatrolPoint].transform.position);
            //}
        }

        protected void AssessTarget()
        {
            if(Target != null)
                Target = Target.activeInHierarchy ? Target : null;
            else
                Target = tField.Targets.Count > 0 ? tField.Targets[0] : null;
        }

        public override void Kill()
        {
            CancelInvoke("AssessTarget");
            gameObject.SetActive(false);
        }

        public override void OnCollisionEnter2D(Collision2D c)
        {
            string t = c.gameObject.tag;
            if (t == "Terrain" || t == "InvulnTerrain")
                return;

            //var v = rigidbody2D.velocity;
            //rigidbody2D.velocity = c.gameObject.rigidbody2D.velocity;
            //c.gameObject.rigidbody2D.velocity = 2*v;

            if(t == "Player")
            {
                c.gameObject.GetComponent<Entity>().TakeDamage(50, color, gameObject);
                TakeDamage(50, color, gameObject);
            }

        }

        public void OnPause()
        {
            paused = true;
            rigidbody2D.velocity = Vector2.zero;
        }

        public void OnUnpause()
        {
            paused = false;
        }
    }
}


