using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiologyAttr : MonoBehaviour
{
    private int _Hp = 1, _Def = 2, _Atk = 3, _ASpeed = 4, _Mp = 5, _MSpeed = 6, _Speed = 6;
    public int Hp, Mp = 100;
    public int HpMax, MpMax = 100;
    private string Lv;
    private uFantasy.Enum.BiologyType Type;


    void Awake()
    {
        Biology biology = GetComponent<Biology>();
        Lv = biology.Lv;
        Type = biology.Type;
        // LoadDB(); fixme:寫到一半
        Rest();
    }

    private void LoadDB()
    {
        if (Lv == "" || GameDB.Instance.Level.ContainsKey(Lv) == false) return;

        Hp = GetHp();
    }

    private int GetHp()
    {   //fixme:先不管生物的type
        return int.Parse(GameDB.Instance.Level[Lv][_Hp]);
    }

    void Rest()
    {
        Hp = HpMax;
    }


}