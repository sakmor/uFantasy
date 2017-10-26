using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class BiologyAI_Condition
{
    internal BiologyAI BiologyAI;
    private static readonly BiologyAI_Condition _instance = new BiologyAI_Condition();
    public static BiologyAI_Condition Instance { get { return _instance; } }
    private Dictionary<string, Command> Conditions;

    private BiologyAI_Condition()
    {
        Conditions = new Dictionary<string, Command>();
        Conditions.Add("Ally: Status = KO", new Command(Ally_HP_Less, 0.0f));
        Conditions.Add("Ally:HP < 100%", new Command(Ally_HP_Less, 1.0f));
        Conditions.Add("Ally:HP < 90%", new Command(Ally_HP_Less, 0.9f));
        Conditions.Add("Ally:HP < 80%", new Command(Ally_HP_Less, 0.8f));
        Conditions.Add("Ally:HP < 70%", new Command(Ally_HP_Less, 0.7f));
        Conditions.Add("Ally:HP < 60%", new Command(Ally_HP_Less, 0.6f));
        Conditions.Add("Ally:HP < 50%", new Command(Ally_HP_Less, 0.5f));
        Conditions.Add("Ally:HP < 40%", new Command(Ally_HP_Less, 0.4f));
        Conditions.Add("Ally:HP < 30%", new Command(Ally_HP_Less, 0.3f));
        Conditions.Add("Ally:HP < 20%", new Command(Ally_HP_Less, 0.2f));
        Conditions.Add("Ally:HP < 10%", new Command(Ally_HP_Less, 0.1f));
        Conditions.Add("Ally:Lowest HP", new Command(Ally_HP_Lowest_Point, 0));

        Conditions.Add("Foe:HP = 100%", new Command(Foe_HP_Full, 0.0f));


        Conditions.Add("Foe:HP < 100,000", new Command(Foe_HP_Less_Point, 100000));
        Conditions.Add("Foe:HP < 50,000", new Command(Foe_HP_Less_Point, 50000));
        Conditions.Add("Foe:HP < 10,000", new Command(Foe_HP_Less_Point, 10000));
        Conditions.Add("Foe:HP < 5,000", new Command(Foe_HP_Less_Point, 5000));
        Conditions.Add("Foe:HP < 3,000", new Command(Foe_HP_Less_Point, 3000));
        Conditions.Add("Foe:HP < 2,000", new Command(Foe_HP_Less_Point, 2000));
        Conditions.Add("Foe:HP < 1,000", new Command(Foe_HP_Less_Point, 1000));
        Conditions.Add("Foe:Lowest HP", new Command(Foe_HP_Lowest_Point, 0));

        Conditions.Add("Foe:HP < 500", new Command(Foe_HP_Less_Point, 500));

        Conditions.Add("Self:HP < 100%", new Command(Self_HP_Less, 1.0f));
        Conditions.Add("Self:HP < 90%", new Command(Self_HP_Less, 0.9f));
        Conditions.Add("Self:HP < 80%", new Command(Self_HP_Less, 0.8f));
        Conditions.Add("Self:HP < 70%", new Command(Self_HP_Less, 0.7f));
        Conditions.Add("Self:HP < 60%", new Command(Self_HP_Less, 0.6f));
        Conditions.Add("Self:HP < 50%", new Command(Self_HP_Less, 0.5f));
        Conditions.Add("Self:HP < 40%", new Command(Self_HP_Less, 0.4f));
        Conditions.Add("Self:HP < 30%", new Command(Self_HP_Less, 0.3f));
        Conditions.Add("Self:HP < 20%", new Command(Self_HP_Less, 0.2f));
        Conditions.Add("Self:HP < 10%", new Command(Self_HP_Less, 0.1f));

        Conditions.Add("Self:MP < 100%", new Command(Self_MP_Less, 1.0f));
        Conditions.Add("Self:MP < 90%", new Command(Self_MP_Less, 0.9f));
        Conditions.Add("Self:MP < 80%", new Command(Self_MP_Less, 0.8f));
        Conditions.Add("Self:MP < 70%", new Command(Self_MP_Less, 0.7f));
        Conditions.Add("Self:MP < 60%", new Command(Self_MP_Less, 0.6f));
        Conditions.Add("Self:MP < 50%", new Command(Self_MP_Less, 0.5f));
        Conditions.Add("Self:MP < 40%", new Command(Self_MP_Less, 0.4f));
        Conditions.Add("Self:MP < 30%", new Command(Self_MP_Less, 0.3f));
        Conditions.Add("Self:MP < 20%", new Command(Self_MP_Less, 0.2f));
        Conditions.Add("Self:MP < 10%", new Command(Self_MP_Less, 0.1f));
    }

    private bool Foe_HP_Lowest_Point(float n)
    {
        return n_HP_Lowest_Point(BiologyAI.Visible_Ally_Biologys, n);
    }
    private bool Ally_HP_Lowest_Point(float n)
    {
        return n_HP_Lowest_Point(BiologyAI.Visible_Ally_Biologys, n);
    }
    private bool n_HP_Lowest_Point(List<Biology> biologys, float n)
    {
        float Hp_Hp_Lowest = Mathf.Infinity;
        for (int i = 0; i < biologys.Count; i++)
        {
            Biology p = biologys[i];
            if (IsBiologyDead(p)) continue;

            if (p.BiologyAttr.Hp > Hp_Hp_Lowest) continue;
            Hp_Hp_Lowest = p.BiologyAttr.Hp;
            BiologyAI.Parent.Target = p;

        }
        return true;
    }

    public void Condition(BiologyAI Ai)
    {
        BiologyAI = Ai;
        BiologyAI.Parent.Target = null;
        for (int i = 0; i < Ai.ConditionList.Count; i++)
        {
            if (Conditions.ContainsKey(Ai.ConditionList[i]) == false) continue;
            Func<float, bool> f = Conditions[Ai.ConditionList[i]].Func;
            float p = Conditions[Ai.ConditionList[i]].p1;
            bool ConditionResult = f(p);
            if (ConditionResult) return;
        }
    }
    private bool Foe_HP_Full(float n)
    {
        for (int i = 0; i < BiologyAI.Visible_Foe_Biologys.Count; i++)
        {
            Biology p = BiologyAI.Visible_Foe_Biologys[i];
            if ((float)p.BiologyAttr.Hp < (float)p.BiologyAttr.HpMax) continue;
            BiologyAI.Parent.Target = p;
            return true;
        }
        return false;
    }
    private bool Ally_HP_Less(float n)
    {
        return n_HP_Less(BiologyAI.Visible_Ally_Biologys, n);
    }
    private bool Foe_HP_Less(float n)
    {
        return n_HP_Less(BiologyAI.Visible_Foe_Biologys, n);
    }
    private bool Self_HP_Less(float n)
    {
        List<Biology> b = new List<Biology>();
        b.Add(BiologyAI.Parent);
        return n_HP_Less(b, n);
    }
    private bool Self_MP_Less(float n)
    {
        List<Biology> b = new List<Biology>();
        b.Add(BiologyAI.Parent);
        return n_Mp_Less(b, n);
    }

    private bool n_Mp_Less(List<Biology> biologys, float n)
    {
        for (int i = 0; i < biologys.Count; i++)
        {
            Biology p = biologys[i];
            if (IsBiologyDead(p)) continue;

            if ((float)p.BiologyAttr.Mp / (float)p.BiologyAttr.MpMax > n) continue;
            BiologyAI.Parent.Target = p;
            return true;
        }
        return false;
    }
    private bool n_HP_Less(List<Biology> biologys, float n)
    {
        for (int i = 0; i < biologys.Count; i++)
        {
            Biology p = biologys[i];
            if (IsBiologyDead(p)) continue;

            if ((float)p.BiologyAttr.Hp / (float)p.BiologyAttr.HpMax > n) continue;
            BiologyAI.Parent.Target = p;
            return true;
        }

        return false;
    }
    private bool Foe_HP_Less_Point(float n)
    {
        return n_HP_Less_Point(BiologyAI.Visible_Foe_Biologys, n);
    }
    private bool n_HP_Less_Point(List<Biology> biologys, float n)
    {
        for (int i = 0; i < biologys.Count; i++)
        {
            Biology p = biologys[i];
            if ((float)p.BiologyAttr.Hp > n) continue;
            BiologyAI.Parent.Target = p;
            return true;
        }
        return false;
    }
    private class Command
    {
        public Func<float, bool> Func;
        public float p1;
        public Command(Func<float, bool> Func, float p1)
        {
            this.Func = Func;
            this.p1 = p1;
        }
    }
    private bool IsBiologyDead(Biology biology)
    {
        if (biology.BiologyAttr.Hp <= 0) return true;
        return false;

    }
}