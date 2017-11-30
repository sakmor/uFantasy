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
    private float _ReturnPostTime;
    private float Closest = 0.125f;
    internal enum MoveType { Run, Back, Stop }
    internal MoveType CurrentMoveType;

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
        GoalPos = BiologyTransfrom.position;
        NavMeshAgent.isStopped = true;
        Biology.PlayAnimation(uFantasy.Enum.State.Battle);
        SetMoveType_Stop();
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
        Biology.Animator.speed = 1 + NavMeshAgent.velocity.magnitude * 0.08f;
    }
    public bool MoveTo(Vector3 pos)
    {
        if (IsPathReachDestination(pos) == false) return false;
        SetGoalPos(pos);
        SetMoveType_MoveTo();
        return _MoveTo();
    }

    private void SetGoalPos(Vector3 pos)
    {
        GoalPos = pos;
    }

    public bool _MoveTo()
    {
        NavMeshAgent.SetDestination(GoalPos);
        NavMeshAgent.isStopped = false;
        Biology.PlayAnimation(uFantasy.Enum.State.Run);
        return true;
    }

    internal void ReturnPost()
    {
        SetMoveType_ReturnPost();
        _MoveTo();
    }
    private bool IsPathReachDestination(Vector3 GoalPos)
    {
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        NavMeshAgent.CalculatePath(GoalPos, path);
        if (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial) return false;

        return true;
    }

    private void SetMoveType_MoveTo()
    {
        CurrentMoveType = MoveType.Run;
        SetAvoidancePriority(97);
    }
    private void SetMoveType_Stop()
    {
        CurrentMoveType = MoveType.Stop;
        SetAvoidancePriority(99);

    }
    private void SetMoveType_ReturnPost()
    {
        CurrentMoveType = MoveType.Back;
        SetAvoidancePriority(98);
    }

    internal float GetSteeringTargetDist()
    {
        return Vector3.Distance(Biology.transform.position, NavMeshAgent.steeringTarget);
    }

    internal void SetAvoidancePriority(int value)
    {
        NavMeshAgent.avoidancePriority = value;
    }
}
