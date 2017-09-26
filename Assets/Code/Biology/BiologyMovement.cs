using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiologyMovement
{
    UnityEngine.AI.NavMeshAgent NavMeshAgent;
    Transform BiologyTransfrom;
    Biology Biology;
    Vector3 GoalPos;

    private float Closest = 0.125f;
    private float Speed = 4.5f;
    public BiologyMovement(Biology biology)
    {
        NavMeshAgent = biology.GetComponent<UnityEngine.AI.NavMeshAgent>();
        NavMeshAgent.stoppingDistance = Closest;
        NavMeshAgent.speed = Speed;
        BiologyTransfrom = biology.transform;
        Biology = biology;
        Biology.Invoke("GoRandom", 0);
        // Stop();
    }



    public void Stop()
    {
        GoalPos = BiologyTransfrom.localPosition;
        NavMeshAgent.isStopped = true;
        Biology.setAction(uFantasy.Enum.State.Battle);
        UpdateDestination();
    }


    public void Update()
    {
        UpdateBiologyState();
        Move();
    }

    private void UpdateBiologyState()
    {
        Biology.State = (uFantasy.Enum.State)Biology.Animator.GetInteger("State");
    }

    private void Move()
    {
        if (NavMeshAgent.remainingDistance < Closest)
        {
            Stop();
            Biology.Invoke("GoRandom", 3);
            return;
        }
    }

    public void MoveTo(Vector3 pos)
    {
        GoalPos = new Vector3(pos.x, 0.5f, pos.z);
        UpdateDestination();
    }

    private void UpdateDestination()
    {
        if (Vector3.Distance(BiologyTransfrom.position, GoalPos) < Closest) return;
        NavMeshAgent.isStopped = false;
        NavMeshAgent.destination = GoalPos;
        Biology.setAction(uFantasy.Enum.State.Run);

    }
}