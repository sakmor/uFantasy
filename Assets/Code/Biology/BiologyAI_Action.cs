using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class BiologyAI_Action
{
    internal BiologyAI_Condition BiologyAI_Condition;
    private static readonly BiologyAI_Action _instance = new BiologyAI_Action();
    public static BiologyAI_Action Instance { get { return _instance; } }
    private Dictionary<string, Command> Actions;
    private string Action = "";
    private Biology Target;

    private BiologyAI_Action()
    {
        Actions = new Dictionary<string, Command>();
        Actions.Add("Attack", new Command(Attack, 0.0f));
        Actions.Add("Magic:Heal", new Command(Magic_Heal, 0.0f));
        Actions.Add("Item:Phoenix Down", new Command(Item_PhoenixDown, 0.0f));
    }
    public bool CheckAction(BiologyAI_Condition BiologyAI_Condition)
    {
        this.BiologyAI_Condition = BiologyAI_Condition;
        Action = BiologyAI_Condition.Action;
        Target = BiologyAI_Condition.Target;

        //如果該行為不在清單內跳出
        if (IsActionInList() == false) return false;

        //回報該行為是否可執行
        Func<float, bool> f = Actions[Action].Func;
        float cp = Actions[Action].p1;
        bool ConditionResult = f(cp);
        return ConditionResult;
    }
    private bool IsActionInList()
    {
        return Actions.ContainsKey(Action);
    }

    private bool Attack(float n)
    {
        //如果攻擊目標是死人則回傳false
        if (IsTargetDead()) return false;

        //如果自己動作不能(石化、混亂...)則回傳false

        return true;
    }
    private bool Magic_Heal(float n)
    {
        //如果目標是死人則回傳false
        if (IsTargetDead()) return false;

        //如果自己動作不能(石化、混亂...)則回傳false

        //如果目標滿血則不施展治療
        if (Target.BiologyAttr.Hp == Target.BiologyAttr.HpMax) return false;


        return true;
    }

    private bool Item_PhoenixDown(float n)
    {
        //如果目標是活人則回傳false
        if (IsTargetDead() == false) return false;

        //如果身上沒有 PhoenixDown 則回傳false

        return false;
    }
    private bool IsTargetDead()
    {
        if (Target.BiologyAttr.Hp <= 0) return true;
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
}