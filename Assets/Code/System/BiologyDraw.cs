using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiologyDraw
{
    public GameDB GameDB = new GameDB(); //fixme:暫代
    public Mesh Mesh;
    public string ModelName;
    public Texture Texture;
    public float Scale;

    private int _Mesh = 1, _Texture = 2, _Scale = 3, _CollisionPostionY = 4;
    private string DrawNum;

    public BiologyDraw(string DrawNum)
    {
        //如果無此資料
        if (GameDB.biologyDraw.ContainsKey(DrawNum) == false)
        {
            return;
        }
        Debug.ClearDeveloperConsole();

        this.DrawNum = DrawNum;
        Mesh = GetDrawMesh();
        Texture = GetDrawTexture();
        Scale = GetDrawScale();
        ModelName = GetModelName();


    }
    private string GetModelName()
    {
        return GameDB.biologyDraw[DrawNum][_Mesh];
    }
    private Mesh GetDrawMesh()
    {
        string MeshName = GameDB.biologyDraw[DrawNum][_Mesh];
        return (Mesh)Resources.Load("Biology/" + MeshName, typeof(Mesh)); //fixme: 不該限制b開頭
    }

    private Texture GetDrawTexture()
    {
        string textureName = GameDB.biologyDraw[DrawNum][_Texture];
        return (Texture)Resources.Load("Biology/Texture/" + textureName, typeof(Texture)); //fixme: 不該限制b開頭
    }
    private float GetDrawScale()
    {
        return float.Parse(GameDB.biologyDraw[DrawNum][_Scale]);
    }

}