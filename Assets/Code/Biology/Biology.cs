using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[SelectionBase]
[DisallowMultipleComponent]

public class Biology : MonoBehaviour
{

    [Header("生物編號")] public string BiologyNum = null;
    [Header("生物名稱")] private string Nname;
    [Header("生物名稱")] private string Name;
    [Header("生物圖號")] private string DrawNum;
    [Header("生物類型")] private uFantasy.Enum.BiologyType Type;
    [Header("生物等級")] private int Lv;
    [Header("AI編號")] private int Ai;
    private Dictionary<int, string[]> BiologyDB;

    [Header("生物模型")] private string ModelName;
    [Header("武器模型")] public GameObject Weapon;//fixme:暫代 應該改為讀取狀態資料

    private GameObject _model, Shadow; //fixme:這個有點壞設計
    public Animator Animator;
    public BiologyMovement BiologyMovement;
    private BiologyAnimator BiologyAnimator;
    [SerializeField] public uFantasy.Enum.State State;

    // Use this for initialization
    private void Awake()
    {
        LoadDB();
    }

    public void setAction(uFantasy.Enum.State state)
    {
        State = state;
        switch (state)
        {
            case uFantasy.Enum.State.Run:
                if (Animator.GetInteger("State") == 600) break;
                Animator.SetInteger("State", 600);
                Animator.CrossFade("Run", 0.25f);
                break;
            case uFantasy.Enum.State.Battle:
                if (Animator.GetInteger("State") == 0) break;
                Animator.SetInteger("State", 0);
                Animator.CrossFade("Battle", 0.25f);
                break;
            case uFantasy.Enum.State.Attack_01:
                BiologyMovement.Stop();
                Animator.CrossFade("Attack_01", 0.25f);
                Animator.SetInteger("State", 101);
                break;
        }
    }


    private void Update()
    {
        BiologyMovement.Update();
        BiologyAnimator.Update();
    }

    public void LoadDB()
    {
        RemoveChild();
        transform.localScale = Vector3.one;
        SetBiology();
        SetBiologyDraw();
        SetBiologyModel();
        SetBiologyWeaponModel();
        SetBiologyAnimator();
        Rename();
        AddShadow();
        SetBiologyMovement();
    }

    private void SetBiologyMovement()
    {
        BiologyMovement = new BiologyMovement(this);
    }

    private void AddShadow()
    {
        Shadow = Instantiate(Resources.Load("Biology/Shadow") as GameObject);
        Shadow.transform.SetParent(transform);
        Shadow.transform.localPosition = Vector3.zero;
        Shadow.name = "Shadow";
    }

    private void RemoveChild()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void SetBiologyWeaponModel()
    {
        if (this.Weapon == null || FindChild("mixamorig_RightHand") == null) return;
        GameObject Weapon = Instantiate(this.Weapon);
        Weapon.transform.SetParent(FindChild("mixamorig_RightHand"));
        Weapon.transform.localEulerAngles = Vector3.zero;
        Weapon.transform.localPosition = Vector3.zero;
    }

    private void Rename()
    {
        name = BiologyNum + " " + Name;
    }

    private void SetBiologyAnimator()
    {
        BiologyAnimator = new BiologyAnimator(this);
        if (_model == null) return;
        Animator = _model.GetComponent<Animator>();
        if (Animator == null) Animator = _model.AddComponent<Animator>();
        Animator.runtimeAnimatorController = Resources.Load("Biology/Motions/" + ModelName) as RuntimeAnimatorController;
        if (Animator.runtimeAnimatorController == null && BiologyNum != "99999") Debug.Log("Controller is Missing !");
    }

    private void SetBiologyModel()
    {
        BiologyModel BiologyModel = new BiologyModel(ModelName);
        GetComponent<BoxCollider>().center = Vector3.up * BiologyModel.CollisionPostionY;
        GetComponent<BoxCollider>().size = new Vector3(BiologyModel.CollisionSizeXZ, 1, BiologyModel.CollisionSizeXZ);
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
        //清空所有子物件
        foreach (Transform child in transform) { DestroyImmediate(child.gameObject); }

        //DrawNum 資料未指定時跳出
        if (DrawNum == null)
        {
            return;
        }

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

    private Transform FindChild(string childName)
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
            if (child.name == childName) return child;

        Debug.Log(" Find Nothing !: " + childName);
        return null;
    }
    public void DestroyGameObject()
    {
        DestroyImmediate(gameObject);
    }

}
