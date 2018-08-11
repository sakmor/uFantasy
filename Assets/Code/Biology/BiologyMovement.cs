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
    internal bool IsStartFaceTarget;
    internal bool IsInputMoving;
    private Transform TargetTransform;
    internal float WalkStep;
    private int _AvoidancePriority;

    public BiologyMovement(Biology biology)
    {
        Biology = biology;
        WalkStep = biology.BiologyLook.WalkStep;
        BiologyTransfrom = biology.transform;
        NavMeshAgent = biology.GetComponent<UnityEngine.AI.NavMeshAgent>();
        NavMeshAgent.stoppingDistance = Closest;
        NavMeshAgent.speed = Biology.Speed;
        _AvoidancePriority = biology.Type == uFantasy.Enum.BiologyType.Player ? 50 : 99;
        Stop();

    }

    internal void Stop()
    {
        GoalPos = BiologyTransfrom.position;
        NavMeshAgent.isStopped = true;
        Biology.PlayAnimation(uFantasy.Enum.State.Battle);
        SetMoveType_Stop();
        Biology.HideMovetoProjector();
        IsInputMoving = false;
    }

    public void Dead()
    {
        NavMeshAgent.enabled = false;
    }

    public void Update()
    {
        UpdateBiologyState();
        UpdateFaceTarget();
        Move();
    }

    private void UpdateFaceTarget()
    {
        if (IsStartFaceTarget == false) return;
        if (NavMeshAgent.enabled == true && NavMeshAgent.isStopped == false) return;
        if (Biology.Target == null) return;
        Quaternion targetRotation = Quaternion.LookRotation(Biology.Target.transform.position - Biology.transform.position);        //fixme：這段重複了，浪費效能運算了兩次
        Biology.transform.rotation = Quaternion.Slerp(Biology.transform.rotation, targetRotation, 10.0f * Time.deltaTime);
    }

    internal float GetFaceTargetAngle()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Biology.Target.transform.position - Biology.transform.position);
        float angle = Quaternion.Angle(Biology.transform.rotation, targetRotation);
        return angle;
    }
    internal bool GetIsFaceTarget()
    {
        float angle = GetFaceTargetAngle();
        if (angle > 2.0f) return false;
        return true;
    }

    internal void StartFaceTarget()
    {
        IsStartFaceTarget = true;
    }

    internal void StopFaceTarget()
    {
        IsStartFaceTarget = false;
        TargetTransform = null;
    }
    private void UpdateBiologyState()
    {
        Biology.AnimationState = (uFantasy.Enum.State)Biology.Animator.GetInteger("State");
    }

    private void Move()
    {
        if (NavMeshAgent.enabled == false) return;
        if (Vector3.Distance(BiologyTransfrom.position, GoalPos) < Closest) Stop();

        if (NavMeshAgent.isStopped) return;
        if (Biology.AnimationState == uFantasy.Enum.State.Battle) Biology.PlayAnimation(uFantasy.Enum.State.Run);
        RunFpsAdjustment();
    }

    private void RunFpsAdjustment()
    {
        Biology.Animator.speed = 1 + NavMeshAgent.velocity.magnitude * WalkStep;
    }



    private void SetGoalPosHitGround()
    {
        float x, y, z;
        x = GoalPos.x; y = GoalPos.y + 1; z = GoalPos.z;
        Vector3 origin = new Vector3(x, y, z);
        RaycastHit hit;
        Physics.Raycast(origin, Vector3.down, out hit);
        SetGoalPos(hit.point);
        Biology.DeactivateMovetoProjector();
    }
    //三種移動之一：AI叫生物移動
    public void ActionMoveto(Vector3 pos)
    {
        if (IsPathReachDestination(pos) == false) return;

        SetGoalPos(pos);
        SetGoalPosHitGround();
        SetMoveType_MoveTo();
        Biology.ActiveMovetoProjector();
        Biology.SetMovetoProjectorPos(pos);
        Biology.DeactivateMovetoProjector();

        MoveTo();
    }


    //三種移動之二：玩家叫生物移動
    public void InputMoveto(Vector3 pos)
    {
        if (IsPathReachDestination(pos) == false) return;

        IsInputMoving = true;

        SetGoalPos(pos);
        SetGoalPosHitGround();
        SetMoveType_MoveTo();
        Biology.ActiveMovetoProjector();
        Biology.SetMovetoProjectorPos(pos);

        MoveTo();
    }

    //三種移動之三：被撞開的生物移動
    internal void ReturnPostMoveto()
    {
        if (IsPathReachDestination(GoalPos) == false) return;

        Biology.DeactivateMovetoProjector();
        SetMoveType_ReturnPost();

        MoveTo();
    }

    public void MoveTo()
    {
        NavMeshAgent.SetDestination(GoalPos);
        NavMeshAgent.isStopped = false;
        // Biology.PlayAnimation(uFantasy.Enum.State.Run);
    }

    private void SetGoalPos(Vector3 pos)
    {
        GoalPos = pos;
    }

    private bool IsPathReachDestination(Vector3 GoalPos)
    {
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        // NavMeshAgent.CalculatePath(GoalPos, path);
        if (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial) return false;

        return true;
    }

    private void SetMoveType_MoveTo()
    {
        CurrentMoveType = MoveType.Run;
        AddAvoidancePriority(-3);
    }
    private void SetMoveType_Stop()
    {
        CurrentMoveType = MoveType.Stop;
        AddAvoidancePriority(0);

    }
    private void SetMoveType_ReturnPost()
    {
        CurrentMoveType = MoveType.Back;
        AddAvoidancePriority(-2);
    }

    internal float GetSteeringTargetDist()
    {
        return Vector3.Distance(Biology.transform.position, NavMeshAgent.steeringTarget);
    }

    internal void SetAvoidancePriority(int value)
    {
        NavMeshAgent.avoidancePriority = value;
    }

    internal void AddAvoidancePriority(int value)
    {
        NavMeshAgent.avoidancePriority = _AvoidancePriority + value;
    }
}
