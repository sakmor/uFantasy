using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biology : MonoBehaviour
{


    [Header("生物編號")] public string BiologyNum = null;
    [Header("生物名稱")] public string Name;
    [Header("生物圖號")] public string DrawNum;
    [Header("生物類型")] public uFantasy.Enum.BiologyType Type;
    [Header("生物等級")] public int Lv;
    [Header("AI編號")] public int Ai;
    private Dictionary<int, string[]> BiologyDB;
    [Header("生物模型")] public string ModelName;


    // Use this for initialization
    private void Start()
    {
        LoadDB();

    }


    // Update is called once per frame
    private void Update()
    {

    }


    public void LoadDB()
    {
        SetBiology();
        SetBiologyDraw();
        SetBiologyModel();

    }

    private void SetBiologyModel()
    {
        BiologyModel BiologyModel = new BiologyModel(ModelName);
        GetComponent<BoxCollider>().center = Vector3.up * BiologyModel.CollisionPostionY;
    }

    private void SetBiology()
    {
        BiologyBuilder BiologyBuilder = new BiologyBuilder(BiologyNum);
        Name = BiologyBuilder.Name;
        DrawNum = BiologyBuilder.DrawNum;

        Type = BiologyBuilder.Type;
        Lv = BiologyBuilder.Lv;
        Ai = BiologyBuilder.Ai;
    }

    private void SetBiologyDraw()
    {
        BiologyDraw BiologyDraw = new BiologyDraw(DrawNum);
        ModelName = BiologyDraw.ModelName;
        GetComponent<MeshFilter>().mesh = BiologyDraw.Mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Unlit/Texture"));
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = BiologyDraw.Texture;
        transform.localScale = Vector3.one * BiologyDraw.Scale;

    }
}
