using System.Collections.Generic;
using UnityEngine;
using System;
public class BiologyAI_Condition
{
    private static readonly BiologyAI_Condition _instance = new BiologyAI_Condition();
    public static BiologyAI_Condition Instance { get { return _instance; } }
    Dictionary<string, Func<bool>> Conditions;

    private BiologyAI_Condition()
    {
        Conditions = new Dictionary<string, Func<bool>>();
        Conditions.Add("Ally:HP < 90%", Ally_HP_Less_90_Percent);
        Conditions.Add("Ally:HP < 80%", Ally_HP_Less_80_Percent);
    }

    public bool Condition(BiologyAI Ai, string n)
    {
        return true;
    }

    private static bool Ally_HP_Less_90_Percent()
    {
        return true;
    }
    private static bool Ally_HP_Less_80_Percent()
    {
        return true;
    }
    private static bool Ally_HP_Less_N_Percent()
    {
        return true;
    }
}