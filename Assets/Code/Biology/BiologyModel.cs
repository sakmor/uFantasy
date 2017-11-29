using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BiologyModel
{
    public float WalkStep;
    public float CollisionSizeXZ;
    public float CollisionPostionY;
    public float CollisionHeight;
    private int _WalkStep = 1;
    private int _CollisionSizeXZ = 2;
    private int _CollisionPostionY = 3;
    private int _CollisionHeight = 4;
    private string ModelName;
    public BiologyModel(string ModelName)
    {
        //如果無此資料
        if (ModelName == "" || GameDB.Instance.BiologyModel.ContainsKey(ModelName) == false)
        {
            return;
        }
        this.ModelName = ModelName;
        WalkStep = GetWalkStep();
        CollisionSizeXZ = GetCollisionPostionXZ();
        CollisionPostionY = GetCollisionPostionY();
        CollisionHeight = GetCollisionHeight();
    }

    private float GetWalkStep()
    {
        return float.Parse(GameDB.Instance.BiologyModel[ModelName][_WalkStep]);
    }
    private float GetCollisionPostionXZ()
    {
        return 0.1f * float.Parse(GameDB.Instance.BiologyModel[ModelName][_CollisionSizeXZ]);
    }
    private float GetCollisionPostionY()
    {
        var i = GameDB.Instance.BiologyModel[ModelName][_CollisionPostionY];
        return 0.1f * float.Parse(i);
    }
    private float GetCollisionHeight()
    {
        return 0.1f * float.Parse(GameDB.Instance.BiologyModel[ModelName][_CollisionHeight]);
    }
}