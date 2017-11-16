using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BiologyMovement
{
    public UnityEngine.AI.NavMeshAgent NavMeshAgent; //fixme:暴露在外面似乎不太好
    private Transform BiologyTransfrom;
    private Biology Biology;
    private Vector3 GoalPos;

    private float Closest = 0.125f;

    public BiologyMovement(Biology biology)
    {
        Biology = biology;
        BiologyTransfrom = biology.transform;
        NavMeshAgent = biology.GetComponent<UnityEngine.AI.NavMeshAgent>();
        NavMeshAgent.stoppingDistance = Closest;
        NavMeshAgent.speed = Biology.Speed;
        Stop();

    }



    public void Stop()
    {
        GoalPos = BiologyTransfrom.localPosition;
        NavMeshAgent.isStopped = true;
        Biology.PlayAnimation(uFantasy.Enum.State.Battle);
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
        RunFpsAdjustment();
        if (NavMeshAgent.isStopped == false && Vector3.Distance(BiologyTransfrom.position, GoalPos) < Closest) Stop();
    }

    private void RunFpsAdjustment()
    {
        // Debug.Log(NavMeshAgent.GetComponent<Animation>().name);
        Biology.Animator.speed = 1 + NavMeshAgent.velocity.magnitude * 0.08f;
    }

    public bool MoveTo(Vector3 pos)
    {
        if (IsPathReachDestination(pos) == false) return false;

        GoalPos = pos;
        NavMeshAgent.SetDestination(GoalPos);
        NavMeshAgent.isStopped = false;
        Biology.PlayAnimation(uFantasy.Enum.State.Run);
        return true;
    }
    private bool IsPathReachDestination(Vector3 GoalPos)
    {
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        NavMeshAgent.CalculatePath(GoalPos, path);
        if (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial) return false;

        return true;
    }
}
