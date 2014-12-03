using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Collections.Generic;
using Pathfinding.Util;
public class WaypointPath
{

    Vector3[] waypoints;
    ABPath[] paths;

    int completedPaths = -1;

    public List<Vector3> vectorPath;
    public List<GraphNode> nodePath;

    public delegate void OnWaypointPathComplete(WaypointPath p);

    OnWaypointPathComplete callback;

    public WaypointPath(Vector3[] waypoints, OnWaypointPathComplete callback)
    {
        this.waypoints = waypoints;
        this.callback = callback;
    }

    public bool HasError()
    {
        return completedPaths != paths.Length;
    }

    public void StartPath(OnPathDelegate config = null)
    {
        if (completedPaths != -1) throw new System.Exception("Do not start the path more than once");
        completedPaths = 0;

        paths = new ABPath[waypoints.Length - 1];
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = ABPath.Construct(waypoints[i], waypoints[i + 1], OnPathComplete);
            if (config != null) config(paths[i]);
            //Here you should set all custom parameters for the path
            //E.g tags
            AstarPath.StartPath(paths[i]);
        }
    }

    void OnPathComplete(Path p)
    {
        if (p.error)
        {
            for (int i = 0; i < paths.Length; i++) paths[i].Error();
            Completed();
            return;
        }

        ABPath path = p as ABPath;
        if (path == null) throw new System.Exception("Only ABPaths should be returned to this object");

        completedPaths++;
        if (completedPaths == paths.Length)
        {
            Completed();
        }
    }

    void Completed()
    {
        if (completedPaths == paths.Length)
        {
            vectorPath = ListPool<Vector3>.Claim();
            nodePath = ListPool<GraphNode>.Claim();

            for (int i = 0; i < paths.Length; i++)
            {
                List<Vector3> vp = paths[i].vectorPath;
                List<GraphNode> np = paths[i].path;
                for (int j = 0; j < vp.Count; j++)
                {
                    if (vectorPath.Count == 0 || vectorPath[vectorPath.Count - 1] != vp[j]) vectorPath.Add(vp[j]);
                }
                for (int j = 0; j < np.Count; j++)
                {
                    if (nodePath.Count == 0 || nodePath[nodePath.Count - 1] != np[j]) nodePath.Add(np[j]);
                }
            }
        }

        if (callback != null)
        {
            callback(this);
        }
    }
}