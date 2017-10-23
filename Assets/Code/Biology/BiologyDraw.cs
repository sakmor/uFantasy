using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiologyDraw
{
    public Mesh Mesh;
    public string ModelName;
    public Texture Texture;
    public float Scale;

    private int _Mesh = 1, _Texture = 2, _Scale = 3;
    private string DrawNum;

    public BiologyDraw(string DrawNum)
    {
        //如果無此資料
        if (DrawNum == "" || GameDB.Instance.biologyDraw.ContainsKey(DrawNum) == false)
        {
            return;
        }
        Debug.ClearDeveloperConsole(); //fixme:忘記放這行在這裡幹嘛了? 

        this.DrawNum = DrawNum;
        Mesh = GetDrawMesh();
        Texture = GetDrawTexture();
        Scale = GetDrawScale();
        ModelName = GetModelName();


    }
    private string GetModelName()
    {
        return GameDB.Instance.biologyDraw[DrawNum][_Mesh];
    }
    private Mesh GetDrawMesh()
    {
        string MeshName = GameDB.Instance.biologyDraw[DrawNum][_Mesh];
        return (Mesh)Resources.Load("Biology/" + MeshName, typeof(Mesh));
    }

    private Texture GetDrawTexture()
    {
        string textureName = GameDB.Instance.biologyDraw[DrawNum][_Texture];
        Texture textrue = (Texture)Resources.Load("Biology/Texture/" + textureName, typeof(Texture));
        return textureName == "" ? null : textrue;
    }
    private float GetDrawScale()
    {
        return float.Parse(GameDB.Instance.biologyDraw[DrawNum][_Scale]);
    }

}