using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uFantasy.Enum;
using System;

public class BiologyBuilder//fixme:名字怪怪的
{

    private int _Name = 1, _DrawNum = 2, _Tpye = 3, _Lv = 4, _Ai = 5, _Speed = 6, _Hp = 9, _ASpeed = 12;

    public string Name;
    public string DrawNum;
    public uFantasy.Enum.BiologyType Type;
    public string Lv;
    public string Ai;
    public int Hp;
    private string BiologyNum;
    internal float Speed, ASpeed;

    public BiologyBuilder(string BiologyNum)
    {
        //如果無此資料
        if (BiologyNum == "" || GameDB.Instance.BiologyDB.ContainsKey(BiologyNum) == false) return;

        this.BiologyNum = BiologyNum;
        //轉換Biology資料
        Name = GetBioName();
        DrawNum = GetBioDrawNum();
        Type = GetBioType();
        Lv = GetBioLv();
        Ai = GetBioAi();
        Hp = GetBioHp();
        Speed = GetBioSpeed();
        ASpeed = GetBioASpeed();
    }

    private float GetBioASpeed()
    {
        float result = float.Parse(GameDB.Instance.BiologyDB[BiologyNum][_ASpeed]);
        return result;
    }

    // 創造生物時必須提供 [圖號]

    private float GetBioSpeed()
    {
        float result = float.Parse(GameDB.Instance.BiologyDB[BiologyNum][_Speed]);
        return result;
    }

    private string GetBioAi()
    {
        string result = GameDB.Instance.BiologyDB[BiologyNum][_Ai];
        return result;
    }

    private string GetBioLv()
    {
        return GameDB.Instance.BiologyDB[BiologyNum][_Lv];
    }

    private int GetBioHp()
    {
        return int.Parse(GameDB.Instance.BiologyDB[BiologyNum][_Hp]);
    }

    private uFantasy.Enum.BiologyType GetBioType()
    {
        string t = GameDB.Instance.BiologyDB[BiologyNum][_Tpye];
        if (t == "玩家") return (uFantasy.Enum.BiologyType)1;
        if (t == "小怪") return (uFantasy.Enum.BiologyType)2;
        if (t == "菁英") return (uFantasy.Enum.BiologyType)3;
        if (t == "王怪") return (uFantasy.Enum.BiologyType)4;

        throw new NotImplementedException();
    }

    private string GetBioDrawNum()
    {
        var result = GameDB.Instance.BiologyDB[BiologyNum][_DrawNum];
        return result;
    }

    private string GetBioName()
    {
        string result = GameDB.Instance.BiologyDB[BiologyNum][_Name];
        return result;
    }


}
