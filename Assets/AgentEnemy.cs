using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentEnemy : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;

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
        
        if(Vector2.Distance(target.position, transform.position) < 0.2f) {
            agent.Stop();
        } else {
            agent.Resume();
            agent.SetDestination(target.position);
        }
    }
}
