﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(BiologyAttr))]
[DisallowMultipleComponent]

public class Biology : MonoBehaviour
{
    [Header("生物編號")] public string BiologyNum = null;
    [Header("生物名稱")] private string Nname;
    [Header("生物名稱")] private string Name;
    [Header("生物圖號")] private string DrawNum;
    [Header("生物類型")] internal uFantasy.Enum.BiologyType Type;
    [Header("生物等級")] internal string Lv;
    [Header("AI編號")] private string Ai;
    [SerializeField] [Header("人物移動速度")] internal float Speed = 1.0f;
    [SerializeField] [Header("行動條進度")] internal float ActionProgressbarValue = 0.0f;
    [Header("生物模型")] private string ModelName;
    [Header("武器模型")] public GameObject Weapon;//fixme:暫代 應該改為讀取狀態資料
    [SerializeField] [Header("目標生物")] internal Biology Target;
    [SerializeField] private bool IsDead;
    private GameObject _model, Shadow; //fixme:這個有點壞設計
    private BezierLine BezierLine;
    internal Animator Animator;
    internal BiologyMovement BiologyMovement;
    internal BiologyAttr BiologyAttr;
    internal BiologyAI BiologyAI;
    internal BiologyAction BiologyAction;
    internal Biology[] Biologys;
    internal HpUI HpUI;
    internal CircleLine CircleLine;
    internal SelectUnit SelectUnit;
    internal CapsuleCollider CapsuleCollider;

    private GameObject MovtoProjector;

    [SerializeField] public uFantasy.Enum.State State;

    // Use this for initialization
    private void Awake()
    {
        LoadDB();
        AddLine();
        AddHpUI();
        AddCircleLine();
        SetBiologyAction();
    }

    void Start()
    {
        GetBiologys();
        GetSelectUnit();
    }

    private void GetBiologys()
    {
        Biologys = GameObject.Find("mainGame").GetComponent<mainGame>().Biologys; //fixme: 不知道有沒有更好的寫法
    }
    private void GetSelectUnit()
    {
        SelectUnit = GameObject.Find("mainGame").GetComponent<mainGame>().SelectUnit; //fixme: 不知道有沒有更好的寫法
    }

    public void Update()
    {
        BiologyMovement.Update();
        BiologyAI.Update();//fixme:可以調整思考頻率
        ActionProgressUpdate();
        Line2Target();//fixme:Debug用
    }

    private void ActionProgressUpdate()
    {
        ActionRuning();
        HpUiSliderValueRefresh();
    }

    private void HpUiSliderValueRefresh()
    {
        HpUI.Slider.value = ActionProgressbarValue;
    }

    private void ActionRuning()
    {
        ActionProgressbarValue += BiologyAttr.ASpeed * 0.25f * Time.deltaTime;
        // ActionProgressbarValue = ActionProgressbarValue >= 1 ? 1 : ActionProgressbarValue;
    }

    private void ActionRestart()
    {
        ActionProgressbarValue = 0;
    }

    private void Line2Target() //fixme:Debug用
    {
        if (Target == null) return;

        if (Type == uFantasy.Enum.BiologyType.Player &&
            Target.Type == uFantasy.Enum.BiologyType.Player) BezierLine.Line2Target(transform, Target.transform, "Green");

        if (Type == uFantasy.Enum.BiologyType.Player &&
            Target.Type != uFantasy.Enum.BiologyType.Player) BezierLine.Line2Target(transform, Target.transform, "Blue");

        if (Type != uFantasy.Enum.BiologyType.Player &&
        Target.Type == uFantasy.Enum.BiologyType.Player) BezierLine.Line2Target(transform, Target.transform, "Red");

        if (Type != uFantasy.Enum.BiologyType.Player &&
            Target.Type != uFantasy.Enum.BiologyType.Player) BezierLine.Line2Target(transform, Target.transform, "Purple");

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
        SetBiologyAI();
        SetGameObjectTag();
        SetModelIgnoreProjectorFX();
        Rename();
        AddMovetoProjector();
        // AddShadow();
        SetBiologyMovement();
        SetBiologyAttr();

    }

    private void AddMovetoProjector()
    {
        MovtoProjector = Instantiate(Resources.Load("Prefab/MovetoProjector", typeof(GameObject)) as GameObject);
        MovtoProjector.name = "MovetoProjector";
        DeactivateMovetoProjector();
    }
    internal void HideMovetoProjector()
    {
        if (MovtoProjector.GetComponent<Animator>().gameObject.activeSelf)
        {
            MovtoProjector.GetComponent<Animator>().SetTrigger("Hide");
        }
    }
    internal void ActiveMovetoProjector()
    {
        MovtoProjector.GetComponent<Animator>().Play("Show");
        MovtoProjector.SetActive(true);
    }
    internal void DeactivateMovetoProjector()
    {
        MovtoProjector.SetActive(false);
    }
    internal void SetMovetoProjectorPos(Vector3 pos)
    {
        MovtoProjector.transform.position = pos;
    }

    private void SetModelIgnoreProjectorFX()
    {
        transform.Find("Model/Model").gameObject.layer = 8;
    }

    private void SetGameObjectTag()
    {
        if (Type == uFantasy.Enum.BiologyType.Player) { tag = "Player"; return; }
        if (Type == uFantasy.Enum.BiologyType.Elite) { tag = "Enemy"; return; }
        if (Type == uFantasy.Enum.BiologyType.Monster) { tag = "Enemy"; return; }
        if (Type == uFantasy.Enum.BiologyType.Boss) { tag = "Enemy"; return; }

    }

    private void AddLine()
    {
        GameObject line = Instantiate(Resources.Load("Prefab/BZLine", typeof(GameObject)) as GameObject);
        line.transform.SetParent(GameObject.Find("uComponet/BZLine").transform);
        line.name = name + "BZLine";
        BezierLine = line.GetComponent<BezierLine>();
    }
    private void AddHpUI()
    {
        GameObject HP = null;
        HP = Instantiate(Resources.Load("Prefab/HP", typeof(GameObject)) as GameObject);
        HpUI = HP.GetComponent<HpUI>();
        if (Type == uFantasy.Enum.BiologyType.Player) HpUI.HPValue.sprite = Resources.Load<Sprite>("UI/ui-assets/HpGreen");
        if (Type != uFantasy.Enum.BiologyType.Player) HpUI.HPValue.sprite = Resources.Load<Sprite>("UI/ui-assets/HpRed");
        HP.transform.SetParent(GameObject.Find("Canvas/HP").transform);//fixme:應該跟maingame要位置
        HP.name = name + "HP";

        HpUI.SetBio(this);
        transform.Find("Model").GetComponent<BiologyAnimationEvent>().HpUI = HpUI;
    }
    private void AddCircleLine()
    {
        GameObject c = null;
        c = Instantiate(Resources.Load("Prefab/CircleLine", typeof(GameObject)) as GameObject);
        c.transform.SetParent(transform);
        c.transform.localPosition = Vector3.zero;
        CircleLine = c.GetComponent<CircleLine>();

    }
    private void SetBiologyAttr()
    {
        //讀取Level資料
        BiologyDraw BiologyDraw = new BiologyDraw(DrawNum);
        ModelName = BiologyDraw.ModelName;
        BiologyAttr = gameObject.GetComponent<BiologyAttr>();
    }

    private void SetBiologyAI()
    {
        BiologyAI = new BiologyAI(this, Ai);
    }

    private void SetBiologyAction()
    {
        BiologyAction = new BiologyAction(this);
    }
    internal void GetDamage(int Damage)
    {
        int Def = BiologyAttr.Def;
        int fDamage = Damage - Def;

        if (fDamage <= 0) return;

        BiologyAttr.Hp -= fDamage;
        PlayAnimation(uFantasy.Enum.State.Hurt);

    }

    internal void CheckDead()
    {
        if (BiologyAttr.Hp >= 0)
        {
            IsDead = false;
            return;
        }

        IsDead = true;
        PlayAnimation(uFantasy.Enum.State.Dead);
        SelectUnit.SelectBiologys.Remove(this);
        SelectUnit._SelectBiologysRemove(this);
        BiologyMovement.Dead();
        CapsuleCollider.enabled = false;
        CircleLine.Hide();
    }

    internal void PlayAnimation(uFantasy.Enum.State state)
    {
        State = state;
        switch (state)
        {
            case uFantasy.Enum.State.Dead:
                Animator.SetInteger("State", 302);
                Animator.Play("Deading");
                break;
            case uFantasy.Enum.State.Run:
                if (Animator.GetInteger("State") == 600) break;
                Animator.SetInteger("State", 600);
                Animator.CrossFade("Run", 0.25f);
                break;
            case uFantasy.Enum.State.Battle:
                if (Animator.GetInteger("State") == 0) break;
                if (Animator.GetInteger("State") == 101) break;
                Animator.SetInteger("State", 0);
                Animator.CrossFade("Battle", 0.25f);
                break;
            case uFantasy.Enum.State.Attack_01:
                if (Animator.GetInteger("State") == 302) break;
                Animator.CrossFade("Attack_01", 0.25f);
                Animator.SetInteger("State", 101);
                break;
            case uFantasy.Enum.State.Use:
                if (Animator.GetInteger("State") == 302) break;
                Animator.CrossFade("Use", 0.25f);
                Animator.SetInteger("State", 101);
                break;
            case uFantasy.Enum.State.Hurt:
                if (Animator.GetInteger("State") == 302) break;
                Animator.CrossFade("Hurt", 0.25f);
                Animator.SetInteger("State", 101);
                break;
        }
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
        if (Application.isPlaying) return;
        foreach (Transform child in transform)
        {

            if (!Application.isPlaying)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }

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
        if (_model == null) return;
        Animator = _model.GetComponent<Animator>();
        if (Animator == null) Animator = _model.AddComponent<Animator>();
        Animator.runtimeAnimatorController = Resources.Load("Biology/Motions/" + ModelName) as RuntimeAnimatorController;
        if (Animator.runtimeAnimatorController == null && BiologyNum != "99999") Debug.Log("Controller is Missing !");
    }

    private void SetBiologyModel()
    {
        BiologyModel BiologyModel = new BiologyModel(ModelName);
        CapsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        CapsuleCollider.center = Vector3.up * BiologyModel.CollisionPostionY;
        CapsuleCollider.height = BiologyModel.CollisionHeight;

    }

    private void SetBiology()
    {
        BiologyBuilder BiologyBuilder = new BiologyBuilder(BiologyNum);
        Name = BiologyBuilder.Name;
        DrawNum = BiologyBuilder.DrawNum;
        Type = BiologyBuilder.Type;
        Lv = BiologyBuilder.Lv;
        Ai = BiologyBuilder.Ai;
        Speed = BiologyBuilder.Speed;

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
        _model.name = "Model";

        //掛上 BiologyAnimationEvent
        _model.AddComponent<BiologyAnimationEvent>();

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

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Biology>() == false) return;
        RunHitRun(other);
        BackHitBack(other);
        BackHitRun(other);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<Biology>() == false) return;
        RunHitStop(other);
    }

    void OnTriggerStay(Collider other)
    {
    }

    private void RunHitRun(Collider other)
    {
        if (other.transform.GetComponent<Biology>().BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Run) return;
        if (BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Run) return;
        BiologyMovement otherMovement = other.transform.GetComponent<Biology>().BiologyMovement;
        if (BiologyMovement.GetSteeringTargetDist() <= otherMovement.GetSteeringTargetDist()) BiologyMovement.SetAvoidancePriority(BiologyMovement.NavMeshAgent.avoidancePriority - 1);
    }
    private void BackHitRun(Collider other)
    {
        if (other.transform.GetComponent<Biology>().BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Back) return;
        if (BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Run) return;
        BiologyMovement.SetAvoidancePriority(BiologyMovement.NavMeshAgent.avoidancePriority - 1);
    }


    private void RunHitStop(Collider other)
    {
        if (other.transform.GetComponent<Biology>().BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Run) return;
        if (BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Stop) return;
        BiologyMovement.ReturnPostMoveto();
    }
    private void BackHitStop(Collider other)
    {
        if (other.transform.GetComponent<Biology>().BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Back) return;
        if (BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Back) return;
        //Do Nothing ...

    }
    private void BackHitBack(Collider other)
    {
        if (other.transform.GetComponent<Biology>().BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Back) return;
        if (BiologyMovement.CurrentMoveType != BiologyMovement.MoveType.Back) return;
        BiologyMovement otherMovement = other.transform.GetComponent<Biology>().BiologyMovement;
        if (BiologyMovement.GetSteeringTargetDist() <= otherMovement.GetSteeringTargetDist()) BiologyMovement.Stop();
    }
    public void UI_HideHP()
    {
        HpUI.gameObject.SetActive(false);
    }

    internal void ActionProgressbarValueRest()
    {
        ActionProgressbarValue = 0;
    }
}
