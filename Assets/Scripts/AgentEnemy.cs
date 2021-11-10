using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentEnemy : MonoBehaviour, IMovePosition
{
    private Transform target;
    public NavMeshAgent agent;
    [SerializeField] public bool followPlayer;
    public GameObject leash;
    public MoveWaypoints moveWaypoints;
    public float followTimeInterval = 10f;
    public float followTime;
    public float beginCountdownDist = 10f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        leash = GameObject.FindGameObjectWithTag("Leash");
        target = PlayerController2D.Instance.transform;
        followPlayer = false;
        moveWaypoints = GetComponent<MoveWaypoints>();
        followTime = followTimeInterval;   
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(followTime);
        if (followPlayer) 
        {
            if (moveWaypoints != null && moveWaypoints.enabled == true) moveWaypoints.enabled = false;
            agent.SetDestination(target.position);

            //timer
            //vector3.distance condition added so that if Vlado is on the other side of the map,
            //followTime is not being spent on travel and doesn't run out before Vlado gets even close to Bosko
            //make sure beginCountdownDist large enough that Bosko can't outrun Vlado and make Vlado follow him indefinitely
            if (Vector3.Distance(transform.position, target.transform.position) < beginCountdownDist
                && followTime > 0)
            {
                followTime -= Time.deltaTime;
            }
            else if (followTime < 0)
            {
                followPlayer = false;
                followTime = followTimeInterval;
            }

        } else if (moveWaypoints != null && moveWaypoints.enabled == false) moveWaypoints.enabled = true;
    }
    
    public void SetMovePosition(Vector3 movePosition) {
        agent.SetDestination(movePosition);
    }

}
