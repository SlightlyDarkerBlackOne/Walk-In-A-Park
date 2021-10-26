﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MoveWaypoints : MonoBehaviour {

    [SerializeField] private Transform[] waypointListTransform;
    [SerializeField] private List<Vector3> waypointList;
    private int waypointIndex;
    public float count;

    private void Start() {
        FillWaypointList();
        count = waypointList.Count;
    }
    private void Update() {
        SetMovePosition(GetWaypointPosition());

        float arrivedAtPositionDistance = 1f;
        if (Vector3.Distance(transform.position, GetWaypointPosition()) < arrivedAtPositionDistance) {
            // Reached position
            if ((waypointIndex+1) >= waypointList.Count) { 
                waypointIndex = 0;
            } else {
                //if (waypointIndex - 1 >= waypointList.Count) {
                waypointIndex++;
            }
        }
    }

    private Vector3 GetWaypointPosition() {
        return waypointList[waypointIndex];
    }

    private void SetMovePosition(Vector3 movePosition) {
        GetComponent<IMovePosition>().SetMovePosition(movePosition);
    }
    private void FillWaypointList() {
        foreach(Transform waypoint in waypointListTransform) {
            waypointList.Add(waypoint.position);
        }
    }
}
