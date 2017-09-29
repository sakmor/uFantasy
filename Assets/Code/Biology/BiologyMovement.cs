using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BiologyMovement
{
    public UnityEngine.AI.NavMeshAgent NavMeshAgent;
    Transform BiologyTransfrom;
    Biology Biology;
    Vector3 GoalPos;

    private float Closest = 0.125f;
    private float Speed = 4.5f;
    public BiologyMovement(Biology biology)
    {
        Biology = biology;
        BiologyTransfrom = biology.transform;
        NavMeshAgent = biology.GetComponent<UnityEngine.AI.NavMeshAgent>();
        NavMeshAgent.stoppingDistance = Closest;
        NavMeshAgent.speed = Speed;
        NavMeshAgent.isStopped = false;

    }



    public void Stop()
    {
        GoalPos = BiologyTransfrom.localPosition;
        NavMeshAgent.isStopped = true;
        Biology.setAction(uFantasy.Enum.State.Battle);
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
        Debug.Log(BiologyTransfrom.name + ":" + NavMeshAgent.isStopped);
        if (NavMeshAgent.remainingDistance < Closest && NavMeshAgent.isStopped == false)
        {
            Stop();
            return;
        }
    }

    public void MoveTo(Vector3 pos)
    {
        GoalPos = pos;
        NavMeshAgent.SetDestination(GoalPos);
        NavMeshAgent.isStopped = false;
        Biology.setAction(uFantasy.Enum.State.Run);
    }

}
