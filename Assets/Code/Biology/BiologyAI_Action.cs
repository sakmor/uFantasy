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
    private string Action;
    private Biology Biology, Target;

    private BiologyAI_Action()
    {
        //fixme:灌資料應該有更好的地方，並且應該配合列舉
        Actions = new Dictionary<string, Command>();
        Actions.Add("Attack", new Command(Attack, 0.0f));
        Actions.Add("Magic:Heal", new Command(Magic_Heal, 0.0f));
        Actions.Add("Item:Phoenix Down", new Command(Item_PhoenixDown, 0.0f));
    }

    public bool CheckAction(BiologyAI_Condition BiologyAI_Condition, Biology target)
    {
        if (BiologyAI_Condition.Target == null) return false; //如果目標不存在直接跳出
        if (BiologyAI_Condition.ActionName == null) return false; //如果行為不存在直接跳出

        this.Biology = BiologyAI_Condition.BiologyAI.Parent;
        this.BiologyAI_Condition = BiologyAI_Condition;
        Action = BiologyAI_Condition.ActionName;
        Target = target;

        //如果該行為不在清單內跳出
        if (IsActionInList() == false) return false;

        //回報該行為是否可執行
        Func<float, bool> f = Actions[Action].Func;
        float cp = Actions[Action].p1;
        bool ConditionResult = f(cp);

        //測試用，正常應該在正確執行完成後列印 而非【可執行】就列印
        if (ConditionResult) DebugAction();
        return ConditionResult;
    }
    private bool IsActionInList()
    {
        return Actions.ContainsKey(Action);
    }
    float current;
    private bool Attack(float n)
    {

        //如果攻擊目標是死人則回傳false
        if (IsTargetDead()) return false;

        //如果目標超過我的 "追擊距離"
        //fixme:測試用追擊距離設定為10f，實際需要依據武器做變化
        if (Vector3.Distance(Target.transform.position, Biology.transform.position) > 10f) return false;

        //如果自己動作不能(石化、混亂...)則回傳false
        //fixme:還沒寫

        //一切都沒問題，執行BiologyAction內的攻擊
        Target.BiologyAction.Attack();

        return true;
    }

    private bool Magic_Heal(float n)
    {
        //如果目標是死人則回傳false
        if (IsTargetDead()) return false;

        //如果自己動作不能(石化、混亂...)則回傳false

        //如果目標滿血則不施展治療
        if (Target.BiologyAttr.Hp >= Target.BiologyAttr.HpMax) return false;

        //fixme:測試用
        if (current < 1) { current += Time.deltaTime; return true; }
        current = 0;
        Target.BiologyAttr.Hp += UnityEngine.Random.Range(100, 120);
        Biology.PlayAnimation(uFantasy.Enum.State.Use);

        return true;
    }

    private bool Item_PhoenixDown(float n)
    {
        //如果目標是活人則回傳false
        if (IsTargetDead() == false) return false;

        //如果身上沒有 PhoenixDown 則回傳false

        //fixme:測試用 
        //Target.BiologyAttr.Hp = (int)(Target.BiologyAttr.HpMax * 0.1f);

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

    private void DebugAction()
    {
        // Debug.Log(Biology.name + "  " + Action + " " + Target + " 因 " + BiologyAI_Condition.ConditionName);
    }
}