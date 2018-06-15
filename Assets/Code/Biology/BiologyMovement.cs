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
    private Transform TargetTransform;

    public BiologyMovement(Biology biology)
    {
        Biology = biology;
        BiologyTransfrom = biology.transform;
        NavMeshAgent = biology.GetComponent<UnityEngine.AI.NavMeshAgent>();
        NavMeshAgent.stoppingDistance = Closest;
        NavMeshAgent.speed = Biology.Speed;
        Stop();

    }

    internal void Stop()
    {
        GoalPos = BiologyTransfrom.position;
        NavMeshAgent.isStopped = true;
        Biology.PlayAnimation(uFantasy.Enum.State.Battle);
        SetMoveType_Stop();
        Biology.HideMovetoProjector();
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
        Debug.Log(Biology.name + "：" + angle);
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
        Biology.State = (uFantasy.Enum.State)Biology.Animator.GetInteger("State");
    }

    private void Move()
    {
        if (NavMeshAgent.enabled == false) return;
        RunFpsAdjustment();
        if (NavMeshAgent.isStopped == false && Vector3.Distance(BiologyTransfrom.position, GoalPos) < Closest) Stop();
    }

    private void RunFpsAdjustment()
    {
        Biology.Animator.speed = 1 + NavMeshAgent.velocity.magnitude * 0.08f;
    }

    //使用者叫生物移動
    public bool InputMoveto(Vector3 pos)
    {
        if (IsPathReachDestination(pos) == false) return false;
        SetGoalPos(pos);
        SetGoalPosHitGround();
        SetMoveType_MoveTo();
        Biology.ActiveMovetoProjector();
        Biology.SetMovetoProjectorPos(pos);
        return MoveTo();
    }

    //生物的AI:Action叫生物移動
    public bool ActionMoveto(Vector3 pos)
    {
        if (IsPathReachDestination(pos) == false) return false;
        InputMoveto(pos);
        Biology.DeactivateMovetoProjector();
        return MoveTo();
    }

    //生物的AI:Action叫生物移動
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

    //生物被撞開返回原始位置
    internal void ReturnPostMoveto()
    {
        Biology.DeactivateMovetoProjector();
        SetMoveType_ReturnPost();
        MoveTo();
    }
    private void SetGoalPos(Vector3 pos)
    {
        GoalPos = pos;
    }

    public bool MoveTo()
    {
        NavMeshAgent.SetDestination(GoalPos);
        NavMeshAgent.isStopped = false;
        Biology.PlayAnimation(uFantasy.Enum.State.Run);
        return true;
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
