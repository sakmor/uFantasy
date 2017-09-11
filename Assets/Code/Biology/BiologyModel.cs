using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BiologyModel
{
    public float WalkStep;
    public float CollisionSizeXZ;
    public float CollisionPostionY;
    private int _WalkStep = 1;
    private int _CollisionSizeXZ = 2;
    private int _CollisionPostionY = 3;

    private string ModelName;
    public BiologyModel(string ModelName)
    {
        //如果無此資料
        if (GameDB.Instance.biologyModel.ContainsKey(ModelName) == false)
        {
            return;
        }
        this.ModelName = ModelName;
        WalkStep = GetWalkStep();
        CollisionSizeXZ = GetCollisionPostionXZ();
        CollisionPostionY = GetCollisionPostionY();
    }

    private float GetWalkStep()
    {
        return float.Parse(GameDB.Instance.biologyModel[ModelName][_WalkStep]);
    }
    private float GetCollisionPostionXZ()
    {
        return float.Parse(GameDB.Instance.biologyModel[ModelName][_CollisionSizeXZ]);
    }
    private float GetCollisionPostionY()
    {
        return float.Parse(GameDB.Instance.biologyModel[ModelName][_CollisionPostionY]);
    }
}