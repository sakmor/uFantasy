using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BiologyModel
{
    public GameDB GameDB = new GameDB(); //fixme:暫代
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
        if (GameDB.biologyModel.ContainsKey(ModelName) == false)
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
        return float.Parse(GameDB.biologyModel[ModelName][_WalkStep]);
    }
    private float GetCollisionPostionXZ()
    {
        return float.Parse(GameDB.biologyModel[ModelName][_CollisionSizeXZ]);
    }
    private float GetCollisionPostionY()
    {
        return float.Parse(GameDB.biologyModel[ModelName][_CollisionPostionY]);
    }
}