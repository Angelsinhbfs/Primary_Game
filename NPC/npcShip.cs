using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Core;
using Assets.Scripts.Utility;

namespace Assets.Scripts.NPC
{
    public class npcShip : Entity
    {
        #region Variables
        //pathfinding
        private PolyNavAgent agent;
        public List<Transform> WayPoints;
        private int currWaypoint = 0;
        #endregion

        void Start()
        {
            agent = GetComponent<PolyNavAgent>();
            if (WayPoints.Count == 0) throw new NotImplementedException("Waypoints not set");

            StartCoroutine(TryPath());
        }

        void OnEnable()
        {
            if (WayPoints.Count == 0) throw new NotImplementedException("Waypoints not set");

            agent.SetDestination(WayPoints[0].position);
        }

        public virtual void Update()
        {
            //check to see if paths have been opened up and ship should proceed

        }

        public override void TakeDamage(int dmg, PrimaryEnums.Color color, GameObject Owner)
        {
            HP -= dmg;
            base.TakeDamage(dmg, color, Owner);
        }

        IEnumerator TryPath()
        {
            //if agent is already has a path or is currently finding one dont do anything
            if (agent.pathPending || agent.hasPath) yield return 0;

            //try setting path to next waypoint
            agent.SetDestination(WayPoints[currWaypoint].position);

            //check for a lack of valid path
            if(!agent.pathPending && !agent.hasPath)
            {
                //clear prime goal so it trys to calculate path again
                agent.primeGoal = Vector2.zero;
                //wait a bit then try again
                yield return StartCoroutine(StaticUtilities.Wait(0.75f));
                yield return StartCoroutine(TryPath());
            }

        }

        public void OnDestinationReached()
        {
            //get the next waypoint if current position equals waypoint position
            if((transform.position - WayPoints[currWaypoint].position).magnitude <= 1f) currWaypoint++;

            //if there are no more waypoints then wait
            if (currWaypoint == WayPoints.Count) return;

            //start trying to go to the next waypoint
            StartCoroutine(TryPath());
        }

        public void OnDestinationInvalid()
        {
            StartCoroutine(TryPath());
        }

        public override void Kill()
        {
            gameObject.SetActive(false);
        }
    }
}
