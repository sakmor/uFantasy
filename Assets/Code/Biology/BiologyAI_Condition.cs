using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class BiologyAI_Condition
{
    internal BiologyAI BiologyAI;
    private static readonly BiologyAI_Condition _instance = new BiologyAI_Condition();
    public static BiologyAI_Condition Instance { get { return _instance; } }
    private Dictionary<string, Func<bool>> Conditions;

    private BiologyAI_Condition()
    {
        Conditions = new Dictionary<string, Func<bool>>();
        Conditions.Add("Ally: Status = KO", Ally_Status_Is_KO);
        Conditions.Add("Ally:HP < 100%", Ally_HP_Less_100_Percent);
        Conditions.Add("Ally:HP < 90%", Ally_HP_Less_90_Percent);
        Conditions.Add("Ally:HP < 80%", Ally_HP_Less_80_Percent);
        Conditions.Add("Ally:HP < 70%", Ally_HP_Less_70_Percent);
        Conditions.Add("Ally:HP < 60%", Ally_HP_Less_60_Percent);
        Conditions.Add("Ally:HP < 50%", Ally_HP_Less_50_Percent);
        Conditions.Add("Ally:HP < 40%", Ally_HP_Less_40_Percent);
        Conditions.Add("Ally:HP < 30%", Ally_HP_Less_30_Percent);
        Conditions.Add("Ally:HP < 20%", Ally_HP_Less_20_Percent);
        Conditions.Add("Ally:HP < 10%", Ally_HP_Less_10_Percent);
        Conditions.Add("Self:HP < 100%", Self_HP_Less_100_Percent);
        Conditions.Add("Self:HP < 90%", Self_HP_Less_90_Percent);
        Conditions.Add("Self:HP < 80%", Self_HP_Less_80_Percent);
        Conditions.Add("Self:HP < 70%", Self_HP_Less_70_Percent);
        Conditions.Add("Self:HP < 60%", Self_HP_Less_60_Percent);
        Conditions.Add("Self:HP < 50%", Self_HP_Less_50_Percent);
        Conditions.Add("Self:HP < 40%", Self_HP_Less_40_Percent);
        Conditions.Add("Self:HP < 30%", Self_HP_Less_30_Percent);
        Conditions.Add("Self:HP < 20%", Self_HP_Less_20_Percent);
        Conditions.Add("Self:HP < 10%", Self_HP_Less_10_Percent);

    }

    public bool Condition(BiologyAI Ai)
    {
        BiologyAI = Ai;
        for (int i = 0; i < Ai.ConditionList.Count; i++)
        {
            if (Conditions.ContainsKey(Ai.ConditionList[i]) == false) continue;
            return Conditions[Ai.ConditionList[i]].Invoke();
        }
        return true;
    }
    private bool Ally_Status_Is_KO()
    {
        return Ally_HP_Less_N_Percent(0.0f); ;
    }
    private bool Ally_HP_Less_100_Percent()
    {
        return Ally_HP_Less_N_Percent(0.9f); ;
    }
    private bool Ally_HP_Less_90_Percent()
    {
        return Ally_HP_Less_N_Percent(0.9f); ;
    }
    private bool Ally_HP_Less_80_Percent()
    {
        return Ally_HP_Less_N_Percent(0.8f);
    }
    private bool Ally_HP_Less_70_Percent()
    {
        return Ally_HP_Less_N_Percent(0.7f);
    }
    private bool Ally_HP_Less_60_Percent()
    {
        return Ally_HP_Less_N_Percent(0.6f);
    }
    private bool Ally_HP_Less_50_Percent()
    {
        return Ally_HP_Less_N_Percent(0.5f);
    }
    private bool Ally_HP_Less_40_Percent()
    {
        return Ally_HP_Less_N_Percent(0.4f);
    }
    private bool Ally_HP_Less_30_Percent()
    {
        return Ally_HP_Less_N_Percent(0.3f);
    }
    private bool Ally_HP_Less_20_Percent()
    {
        return Ally_HP_Less_N_Percent(0.2f);
    }
    private bool Ally_HP_Less_10_Percent()
    {
        return Ally_HP_Less_N_Percent(0.1f);
    }
    private bool Self_HP_Less_100_Percent()
    {
        return Self_HP_Less_N_Percent(0.9f); ;
    }
    private bool Self_HP_Less_90_Percent()
    {
        return Self_HP_Less_N_Percent(0.9f); ;
    }
    private bool Self_HP_Less_80_Percent()
    {
        return Self_HP_Less_N_Percent(0.8f);
    }
    private bool Self_HP_Less_70_Percent()
    {
        return Self_HP_Less_N_Percent(0.7f);
    }
    private bool Self_HP_Less_60_Percent()
    {
        return Self_HP_Less_N_Percent(0.6f);
    }
    private bool Self_HP_Less_50_Percent()
    {
        return Self_HP_Less_N_Percent(0.5f);
    }
    private bool Self_HP_Less_40_Percent()
    {
        return Self_HP_Less_N_Percent(0.4f);
    }
    private bool Self_HP_Less_30_Percent()
    {
        return Self_HP_Less_N_Percent(0.3f);
    }
    private bool Self_HP_Less_20_Percent()
    {
        return Self_HP_Less_N_Percent(0.2f);
    }
    private bool Self_HP_Less_10_Percent()
    {
        return Self_HP_Less_N_Percent(0.1f);
    }
    private bool Ally_HP_Less_N_Percent(float n)
    {

        for (int i = 0; i < BiologyAI.Visible_Ally_Biologys.Count; i++)
        {
            Biology p = BiologyAI.Visible_Ally_Biologys[i];
            if ((float)p.BiologyAttr.Hp / (float)p.BiologyAttr.HpMax > n) continue;
            BiologyAI.Parent.Target = p;
            return true;

        }
        BiologyAI.Parent.Target = null;
        return false;
    }
    private bool Self_HP_Less_N_Percent(float n)
    {


        if ((float)BiologyAI.Parent.BiologyAttr.Hp / (float)BiologyAI.Parent.BiologyAttr.HpMax > n)
        {
            BiologyAI.Parent.Target = null;
            return false;
        }

        BiologyAI.Parent.Target = BiologyAI.Parent;
        return true;



    }
}