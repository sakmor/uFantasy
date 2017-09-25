using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiologyMovement
{

    Transform BiologyTransfrom;
    Biology Biology;
    Vector3 GoalPos;

    private float Closest = 0.125f;
    private float Speed = 2.5f;
    public BiologyMovement(Biology biology)
    {
        BiologyTransfrom = biology.transform;
        Biology = biology;
        Stop();
    }

    public void Stop()
    {
        GoalPos = BiologyTransfrom.localPosition;
    }


    public void Update()
    {
        // Biology.State = Biology.Animator.GetInteger("State"); fixme:用這方法更新狀態
        Move();
        FaceToGoal();
        Debug.Log(GoalPos);
    }

    private void FaceToGoal()
    {
        if (isAngleLessClosest()) return;
        Vector3 targetDir = GoalPos - BiologyTransfrom.position;
        Vector3 newDir = Vector3.RotateTowards(BiologyTransfrom.forward, targetDir, Speed * Time.deltaTime, 0.0f);
        BiologyTransfrom.localRotation = Quaternion.LookRotation(newDir);
    }

    private bool isAngleLessClosest()
    {
        float GoalPosAngle = getAngleGoalPos();
        if (GoalPosAngle < Closest) return true;
        if (GoalPosAngle >= Closest) return false;
        return false;
    }

    private float getAngleGoalPos()
    {
        return Vector3.Angle(BiologyTransfrom.position, GoalPos);
    }
    private float getGoalPosDist()
    {
        return Vector3.Distance(BiologyTransfrom.position, GoalPos);
    }

    private bool isDistLessClosest()
    {
        float MovePosDist = getGoalPosDist();
        if (MovePosDist < Closest) return true;
        if (MovePosDist >= Closest) return false;
        return false;
    }

    private void Move()
    {
        if (Biology.State != uFantasy.Enum.State.Run) return;
        if (isDistLessClosest())
        {
            Biology.setAction(uFantasy.Enum.State.Battle);
            return;
        }

        BiologyTransfrom.localPosition = Vector3.MoveTowards(BiologyTransfrom.localPosition, GoalPos, Speed * Time.deltaTime);
        Biology.setAction(uFantasy.Enum.State.Run);
    }

    public void MoveTo(Vector3 pos)
    {
        GoalPos = new Vector3(pos.x, 0.5f, pos.z);

    }
}