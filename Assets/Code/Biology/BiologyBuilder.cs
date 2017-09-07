using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uFantasy.Enum;
using System;

public class BiologyBuilder//fixme:名字怪怪的
{
    public GameDB GameDB = new GameDB(); //fixme:暫代

    private int _Name = 1, _DrawNum = 2, _Tpye = 3, _Lv = 4, _Ai = 5;

    public string Name;
    public int DrawNum;
    public uFantasy.Enum.BiologyType Type;
    public int Lv;
    public int Ai;
    private int BiologyNum;

    public BiologyBuilder(int BiologyNum)
    {
        //如果無此資料
        if (GameDB.biologyDB.ContainsKey(BiologyNum) == false)
        {
            Debug.LogError("查無此生物編號:" + BiologyNum.ToString());
            return;
        }
        this.BiologyNum = BiologyNum;
        //轉換Biology資料
        Name = GetBioName();
        DrawNum = GetBioDrawNum();
        Type = GetBioType();
        Lv = GetBioLv();
        Ai = GetBioAi();
    }

    // 創造生物時必須提供 [圖號]


    private int GetBioAi()
    {
        int result = int.Parse(GameDB.biologyDB[BiologyNum][_Lv]);
        return result;
    }

    private int GetBioLv()
    {
        int result = int.Parse(GameDB.biologyDB[BiologyNum][_Ai]);
        return result;
    }

    private uFantasy.Enum.BiologyType GetBioType()
    {
        var result = int.Parse(GameDB.biologyDB[BiologyNum][_Tpye]);
        return (uFantasy.Enum.BiologyType)result;
    }

    private int GetBioDrawNum()
    {
        var result = int.Parse(GameDB.biologyDB[BiologyNum][_DrawNum]);
        return result;
    }

    private string GetBioName()
    {
        string result = GameDB.biologyDB[BiologyNum][_Name];
        return result;
    }

}
