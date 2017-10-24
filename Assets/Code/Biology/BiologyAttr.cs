using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiologyAttr : MonoBehaviour
{
    public int Hp, Mp = 100;
    public int HpMax, MpMax = 100;
    internal Biology Biology;


    void Awake()
    {
        Biology = GetComponent<Biology>();
        Rest();
    }

    void Rest()
    {
        Hp = HpMax;
    }


}