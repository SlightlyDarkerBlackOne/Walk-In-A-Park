/* 
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

public class MoveRoam : MonoBehaviour {

    public float movementRangeX = 3f;
    public float movementRangeY = 3f;

    public Transform targetToMoveAwayFrom;

    private Vector3 startPosition;
    private Vector3 targetMovePosition;

    private void Awake() {
        startPosition = transform.position;
    }

    private void Start() {
        SetRandomMovePosition();
    }

    private void SetRandomMovePosition() {
        //do {
            targetMovePosition = startPosition + UtilsClass.GetRandomDir() * Random.Range(movementRangeX, movementRangeY);
        //} while (!AngleToPlayer.IsAngleInPlayerDirection(targetMovePosition, targetToMoveAwayFrom, transform));
        
    }

    private void Update() {
        SetMovePosition(targetMovePosition);

        float arrivedAtPositionDistance = 1f;
        if (Vector3.Distance(transform.position, targetMovePosition) < arrivedAtPositionDistance) {
            // Reached position
            SetRandomMovePosition();
        }
    }

    private void SetMovePosition(Vector3 movePosition) {
        GetComponent<IMovePosition>().SetMovePosition(movePosition);
    }


}
