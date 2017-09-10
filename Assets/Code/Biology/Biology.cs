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

    private GameObject _model; //fixme:這個有點壞設計
    private Animator Animator;



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
        SetBiologyAnimator();


    }

    private void SetBiologyAnimator()
    {
        Animator = _model.GetComponent<Animator>();
        Animator.runtimeAnimatorController = Resources.Load("Biology/Controller/" + ModelName) as RuntimeAnimatorController;
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
        //DrawNum 資料未指定時跳出
        if (DrawNum == null) return;

        //清空所有子物件
        foreach (Transform child in transform) { DestroyImmediate(child.gameObject); }

        //讀取圖號資料
        BiologyDraw BiologyDraw = new BiologyDraw(DrawNum);
        ModelName = BiologyDraw.ModelName;

        //載入模型
        _model = Instantiate(Resources.Load("Biology/" + BiologyDraw.ModelName) as GameObject);
        _model.transform.SetParent(transform);
        _model.transform.localPosition = Vector3.zero;
        _model.name = ModelName;

        //載入縮放大小
        transform.localScale = Vector3.one * BiologyDraw.Scale;

        //載入貼圖資訊
        if (BiologyDraw.Texture == null) return;
        SkinnedMeshRenderer SkinnedMeshRenderer = _model.transform.Find("Model").GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Texture")); // fixme:不確定這樣的做法是否會造成記憶體浪費？
        SkinnedMeshRenderer.sharedMaterial.mainTexture = BiologyDraw.Texture;

    }


}
