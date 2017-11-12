using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiologyAttr : MonoBehaviour
{
    private int _Hp = 1, _Def = 2, _Atk = 3, _ASpeed = 4, _Mp = 5, _MSpeed = 6, _Speed = 6;
    private int TypeStep = 7;
    public string Lv;
    public int HpMax, Def, Atk, ASpeed, Mp, MpMax, MSpeed, Speed;
    public int hp;
    internal Biology Biology;

    public uFantasy.Enum.BiologyType Type;

    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            if (hp == value) return;

            hp = value;
            UpdateHpUI();
        }
    }

    void Start()
    {
        Biology = GetComponent<Biology>();
        Lv = Biology.Lv;
        Type = Biology.Type;
        LoadDB();
        Rest();
    }

    internal void UpdateHpUI()
    {
        Biology.HpUI.ChangeHP();
    }

    private void LoadDB()
    {
        if (Lv == "" || GameDB.Instance.Level.ContainsKey(Lv) == false) return;
        TypeStep = ((int)Type - 1) * TypeStep;
        HpMax = GetHpMax();
        Def = GetDef();
        Atk = GetAtk();
        ASpeed = GetASpeed();
        MpMax = GetMpMax();
        MSpeed = GetMSpeed();
        Speed = GetSpeed(); ;
    }

    private int GetSpeed()
    {
        return int.Parse(GameDB.Instance.Level[Lv][_Speed + TypeStep]);
    }

    private int GetMSpeed()
    {
        return int.Parse(GameDB.Instance.Level[Lv][_MSpeed + TypeStep]);
    }

    private int GetMpMax()
    {
        return int.Parse(GameDB.Instance.Level[Lv][_Mp + TypeStep]);
    }

    private int GetASpeed()
    {
        return int.Parse(GameDB.Instance.Level[Lv][_ASpeed + TypeStep]);
    }

    private int GetAtk()
    {
        return int.Parse(GameDB.Instance.Level[Lv][_Atk + TypeStep]);
    }

    private int GetDef()
    {
        return int.Parse(GameDB.Instance.Level[Lv][_Def + TypeStep]);
    }

    private int GetHpMax()
    {
        return int.Parse(GameDB.Instance.Level[Lv][_Hp + TypeStep]);
    }


    private void Update()
    {

    }

    private void Rest()
    {
        Hp = HpMax;
    }




}