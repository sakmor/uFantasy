using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uFantasy.Enum;
using System;

public class BiologyBuilder
{
    public GameDB GameDB = new GameDB(); //fixme:暫代

    private int _Name = 2;
    private int _DrawNum = 3;
    private int _Tpye = 4;
    private int _Lv = 5;
    private int _Ai = 6;
    public void CreateBiology(int biologyNum, Vector3 pos)
    {
        string name = GetBioName(biologyNum);
        int DrawNum = GetBioDrawNum(biologyNum);
        int Type = GetBioType(biologyNum);
        int Lv = GetBioLv(biologyNum);
        int Ai = GetBioAi(biologyNum);
    }

    private int GetBioAi(int biologyNum)
    {
        int result = int.Parse(GameDB.biologyDB[biologyNum][_Lv]);
        return result;
    }

    private int GetBioLv(int biologyNum)
    {
        int result = int.Parse(GameDB.biologyDB[biologyNum][_Ai]);
        return result;
    }

    private int GetBioType(int biologyNum)
    {
        int result = int.Parse(GameDB.biologyDB[biologyNum][_Tpye]);
        return result;
    }

    private int GetBioDrawNum(int biologyNum)
    {
        int result = int.Parse(GameDB.biologyDB[biologyNum][_DrawNum]);
        return result;
    }

    private string GetBioName(int biologyNum)
    {
        string result = GameDB.biologyDB[biologyNum][_Name];
        return result;
    }

}
