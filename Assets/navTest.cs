using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navTest : MonoBehaviour
{
    public Vector3 GoalPos;
    public Vector3 CurrentPos;
    UnityEngine.AI.NavMeshAgent NavMeshAgent;
    // Use this for initialization
    void Start()
    {
        GoalPos = new Vector3(0, 0.5f, 0);
        NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        NavMeshAgent.destination = GoalPos;
    }

    // Update is called once per frame
    void Update()
    {


    }
}
