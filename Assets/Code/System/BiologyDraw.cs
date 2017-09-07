using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiologyDraw
{
    public GameDB GameDB = new GameDB(); //fixme:暫代
    private int _Mesh = 1, _Texture = 2, _Scale = 3;
    public Mesh Mesh;
    public Texture Texture;
    public float Scale;
    private int DrawNum;
    public BiologyDraw(int DrawNum)
    {
        //如果無此資料
        if (GameDB.biologyDraw.ContainsKey(DrawNum) == false)
        {
            Debug.LogError("查無此生物圖號:" + DrawNum.ToString());
            return;
        }
        this.DrawNum = DrawNum;
        Mesh = GetDrawMesh();
        Texture = GetDrawTexture();
        Scale = GetDrawScale();

    }

    private Mesh GetDrawMesh()
    {
        string MeshName = GameDB.biologyDraw[DrawNum][_Mesh];
        return (Mesh)Resources.Load("Biology/b" + MeshName, typeof(Mesh));
    }

    private Texture GetDrawTexture()
    {
        string MeshName = GameDB.biologyDraw[DrawNum][_Texture];
        return (Texture)Resources.Load("Biology/Texture/b" + MeshName, typeof(Texture));
    }
    private float GetDrawScale()
    {
        return float.Parse(GameDB.biologyDraw[DrawNum][_Scale]);
    }
}