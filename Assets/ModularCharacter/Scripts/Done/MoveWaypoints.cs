﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MoveWaypoints : MonoBehaviour {

    [SerializeField] private Transform[] waypointListTransform;
    [HideInInspector]
    [SerializeField] private List<Vector3> waypointList;
    private int waypointIndex;
    private float count;
    //must be greater than whatever stopping distance set in editor!
    private float arrivedAtPositionDistance = 3.5f;

    private void Start() {
        FillWaypointList();
        count = waypointList.Count;
        SetMovePosition(GetWaypointPosition());
    }
    private void Update() {
        ReachedWaypointPosition();
    }
    private void ReachedWaypointPosition() {
        //if we were just let off the leash we need to get moving the still Vlado again
        if (GetComponent<Rigidbody2D>().velocity == Vector2.zero) SetMovePosition(GetWaypointPosition());

        if (Vector3.Distance(transform.position, GetWaypointPosition()) < arrivedAtPositionDistance) {
            // Reached position
            if ((waypointIndex + 1) >= waypointList.Count) {
                waypointIndex = 0;
            } else {
                waypointIndex++;
            }
            SetMovePosition(GetWaypointPosition());
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
