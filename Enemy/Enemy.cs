using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;
using Pathfinding;
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
        public int Speed;

        public GameObject Target;
        public List<Transform> PatrolPoints;
        public TargettingField tField;
        private bool isPointsCollected = false;

        private Seeker seeker;

        //The calculated path
        public Path path;

        //The max distance from the AI to a waypoint for it to continue to the next waypoint
        public float nextWaypointDistance = 6;

        //The waypoint we are currently moving towards
        private int currentWaypoint = 0;

        private int currentPatrolPoint = 0;

        public float repathRate = 0.5f;
        private float lastRepath = -9999;

        // Use this for initialization
        public void Start()
        {
            BuildPatrolPoints();
            //Debug.Log(PatrolPoints);
            seeker = GetComponent<Seeker>();
            PatrolPoints = StaticUtilities.ShuffleWaypointList(PatrolPoints);

            seeker.StartPath(transform.position, PatrolPoints[0].position, OnPathComplete);
            currentPatrolPoint++;
        }

        private void BuildPatrolPoints()
        {
            var t = GameObject.FindGameObjectsWithTag("PatrolPoint");
            //Debug.Log(t.Length);
            for (int i = 0; i < t.Length; i++)
            {
                PatrolPoints.Add(t[i].transform);
            }
            isPointsCollected = true;
        }
        void OnEnable()
        {
            Target = null;
            rigidbody2D.velocity = Vector2.zero;
            HP = MaxHp;
            InvokeRepeating("AssessTarget", 0.1f, 0.5f);
        }

        public override void Move()
        {
            if (!isPointsCollected) return;
            if (currentPatrolPoint == PatrolPoints.Count) currentPatrolPoint = 0;
            
            switch (State)
            {
                case PrimaryEnums.AiState.Wander:
                    if (path == null) return;
                    if (currentWaypoint > path.vectorPath.Count) return;
                    if (currentWaypoint == path.vectorPath.Count)
                    {
                        //Debug.Log("End Of Path Reached");
                        currentWaypoint++;
                        seeker.StartPath(transform.position, PatrolPoints[currentPatrolPoint].position, OnPathComplete);
                        currentPatrolPoint++;
                        return;
                    }
                    var cw = currentWaypoint  < path.vectorPath.Count ? currentWaypoint : currentWaypoint - 1;
                    transform.rotation = StaticUtilities.XYLookRotation(transform.position, path.vectorPath[cw]);
                    //transform.position =  path.vectorPath[cw];
                    rigidbody2D.velocity = transform.up * Speed / 2;
                    break;
                case PrimaryEnums.AiState.Follow:
                    if (Time.time - lastRepath > repathRate && seeker.IsDone())
                    {
                        lastRepath = Time.time + Random.value * repathRate * 0.5f;
                        seeker.StartPath(transform.position, Target.transform.position, OnPathComplete);
                    }

                    if (path == null) return;
                    if (currentWaypoint > path.vectorPath.Count) return;
                    if (currentWaypoint == path.vectorPath.Count)
                    {
                        //Debug.Log("End Of Path Reached");
                        currentWaypoint++;
                        seeker.StartPath(transform.position, Target.transform.position, OnPathComplete);
                    }
                    //rigidbody2D.velocity = transform.up * Speed;
                    transform.rotation =  StaticUtilities.XYLookRotation(transform.position, path.vectorPath[currentWaypoint]);
                    rigidbody2D.velocity = transform.up * Speed;
                    //transform.position = Vector3.Lerp(transform.position, path.vectorPath[currentWaypoint], Speed * Time.deltaTime);
                    break;
                default:
                    break;
            }
            if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance * nextWaypointDistance)
            {
                //Debug.Log("waypoint reached");
                currentWaypoint++;
                return;
            }
            
        }

        public void OnPathComplete(Path p)
        {
            p.Claim(this);
            if (!p.error)
            {
                if (path != null) path.Release(this);
                path = p;
                //Reset the waypoint counter
                currentWaypoint = 0;
            }
            else
            {
                p.Release(this);
                Debug.Log("Oh noes, the target was not reachable: " + p.errorLog);
            }
        }

        

        void AssessTarget()
        {
            //Target = tField.Targets[0];
            if(Target != null)
                Target = Target.activeInHierarchy ? Target : null;
            else
                Target = tField.Targets.Count > 0 ? tField.Targets[0] : null;
            State = Target == null ? PrimaryEnums.AiState.Wander : PrimaryEnums.AiState.Follow;
        }

        public override void Kill()
        {
            CancelInvoke("AssessTarget");
            gameObject.SetActive(false);
        }

        public override void OnCollisionEnter2D(Collision2D c)
        {
            string t = c.collider.tag;
            if (t == "Terrain")
            {
                rigidbody2D.velocity = -rigidbody2D.velocity;
                return;
            }

            var v = rigidbody2D.velocity;
            rigidbody2D.velocity = c.gameObject.rigidbody2D.velocity;
            c.gameObject.rigidbody2D.velocity = v;

            if(t == "Player")
            {
                Debug.Log("player hit");
                c.gameObject.GetComponent<Entity>().TakeDamage(50);
                TakeDamage(50);
            }

            if (t == "Shield")
            {
                Debug.Log("Shield hit");
                c.gameObject.GetComponentInChildren<Shield>().TakeDamage(50, color);
                TakeDamage(50);
            }

        }
    }
}


