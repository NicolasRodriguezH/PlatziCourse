using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class zombie : Enemy
{
    // Lista de transforms donde se determina que va a ir de un punto X a otro Y;
    public List<Transform> waypoints;
    private int index;
    private Vector3 waypointTarget;
    protected override void FixedUpdate()
    {

    }
    private void Awake()
    {
        waypointTarget = waypoints[index].position;
    }
    protected override void PatrolUpdate()
    {
        if (Vector3.Distance(waypointTarget, transform.position) <= 0f)
        {
            index = (index + 1) % waypoints.Count;
        }
        waypointTarget = waypoints[index].position;
        transform.position = Vector3.MoveTowards(transform.position, waypointTarget, Time.deltaTime * speed);
        transform.forward = (waypointTarget -transform.position ).normalized;
    }

    private void OnDrawGizmosSelected() {
        
            Gizmos.color = Color.yellow;
            foreach (var waypoint in waypoints)
            {
                Gizmos.DrawSphere(waypoint.position, 0.1f);
            }
    }
    
}

[CustomEditor(typeof(zombie))]
public class ZombieCustomEditor : Editor
{
    // Uso de handles
    private void OnSceneGUI()
    {
        var zombieObj = (zombie)target;
        for (int i = 0; i < zombieObj.waypoints.Count; i++)
        {
            var waypoint = zombieObj.waypoints[i];
            var nextWaypoint = zombieObj.waypoints[(i + 1) % zombieObj.waypoints.Count];

            if (waypoint == null) continue;
            if (nextWaypoint == null) continue;

            Handles.color = Color.white;
            Handles.DrawDottedLine(waypoint.position, nextWaypoint.position, 6f);

            EditorGUI.BeginChangeCheck();
            var newPos = Handles.PositionHandle(waypoint.position, waypoint.rotation);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(waypoint, "Move Waipoint");
                waypoint.position = newPos;
            }
        }
    }
}
