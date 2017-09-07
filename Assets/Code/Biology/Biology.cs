using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biology : MonoBehaviour
{


    [Header("生物編號")] public int BiologyNum = 1001;
    [Header("生物名稱")] public string Name;
    [Header("生物圖號")] public int DrawNum;
    [Header("生物類型")] public uFantasy.Enum.BiologyType Type;
    [Header("生物等級")] public int Lv;
    [Header("AI編號")] public int Ai;
    private Dictionary<int, string[]> BiologyDB;


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
        BiologyBuilder BiologyBuilder = new BiologyBuilder(BiologyNum);
        Name = BiologyBuilder.Name;
        DrawNum = BiologyBuilder.DrawNum;
        Type = BiologyBuilder.Type;
        Lv = BiologyBuilder.Lv;
        Ai = BiologyBuilder.Ai;

        SetModeTexture();

    }

    private void SetModeTexture()
    {
        BiologyDraw BiologyDraw = new BiologyDraw(DrawNum);
        GetComponent<MeshFilter>().mesh = BiologyDraw.Mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Unlit/Texture"));
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = BiologyDraw.Texture;
        transform.localScale = Vector3.one * BiologyDraw.Scale;
    }
}
