using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BiologyMovement
{
    public bool isMove;
    Transform BiologyTransfrom;
    Vector3 GoalPos;
    float MovePosDist, Closest = 0.125f;
    private float Speed = 10f;
    public BiologyMovement(Transform n)
    {
        BiologyTransfrom = n;
        setStop();
    }

    private void setStop()
    {
        isMove = true;
        GoalPos = BiologyTransfrom.localPosition;
    }
    private void setMove()
    {
        isMove = false;
    }

    public void Update()
    {
        UpdateMovePosDist();
        if (isDistLessClosest()) return;
        Move();
        FaceToGoal();
    }

    private void FaceToGoal()
    {
        BiologyTransfrom.LookAt(GoalPos);
    }

    private void UpdateMovePosDist()
    {
        MovePosDist = Vector3.Distance(BiologyTransfrom.position, GoalPos);
    }

    private bool isDistLessClosest()
    {
        if (MovePosDist < Closest)
        {
            setStop();
            return true;
        }
        if (MovePosDist >= Closest)
        {
            setMove();
            return false;
        }
        return false;
    }

    private void Move()
    {
        BiologyTransfrom.localPosition =
         Vector3.MoveTowards(BiologyTransfrom.localPosition, GoalPos, Speed * Time.deltaTime);
    }

    public void MoveTo(Vector3 pos)
    {
        GoalPos = new Vector3(GoalPos.x, 0.5f, GoalPos.z);
    }
}