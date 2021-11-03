using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentEnemy : MonoBehaviour, IMovePosition
{
    private Transform target;
    private NavMeshAgent agent;
    [SerializeField] private bool followPlayer;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController2D.Instance.transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer) {
            agent.SetDestination(target.position);
        }
    }
    public void SetMovePosition(Vector3 movePosition) {
        agent.SetDestination(movePosition);
    }

}
