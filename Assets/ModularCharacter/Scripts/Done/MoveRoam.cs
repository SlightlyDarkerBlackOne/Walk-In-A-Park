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
using UnityEngine.UI;
using CodeMonkey.Utils;
using UnityEngine.AI;

public class MoveRoam : MonoBehaviour {

    public float npcMoveSpeedMin = 0.8f;
    public float npcMoveSpeedMax = 3f;

    public Transform targetToMoveAwayFrom;

    private Vector3 startPosition;
    private Vector3 targetMovePosition;
    private AgentRoamer agent;
    private NavMeshAgent navmeshAgent;
    private void Awake() {
        startPosition = transform.position;
        agent = GetComponent<AgentRoamer>();
    }

    private void Start() {
        navmeshAgent = GetComponent<NavMeshAgent>();
        SetRandomMovePosition();
    }

    private void SetRandomMovePosition() {
        //targetMovePosition = startPosition + UtilsClass.GetRandomDir() * Random.Range(movementRangeX, movementRangeY);
        targetMovePosition = GetRandomWaypoint();
        navmeshAgent.speed = Random.Range(npcMoveSpeedMin, npcMoveSpeedMax);
    }

    private void Update() {
        agent.SetAgentDestination(targetMovePosition);
        //SetMovePosition(targetMovePosition);

        float arrivedAtPositionDistance = 1f;
        if (Vector3.Distance(transform.position, targetMovePosition) < arrivedAtPositionDistance) {
            // Reached position
            startPosition = transform.position;
            SetRandomMovePosition();

        }
    }

    private void SetMovePosition(Vector3 movePosition) {
        GetComponent<IMovePosition>().SetMovePosition(movePosition);
    }
    Vector3 GetRandomWaypoint() {
        return GameManager.Instance.RandomWaypoint();
    }

}
