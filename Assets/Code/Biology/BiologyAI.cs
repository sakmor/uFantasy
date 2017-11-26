using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BiologyAI
{
    public List<string> ConditionList;
    public List<string> ActionList;
    private int _ConditionListStart = 3, _ConditionListEnd = 25;
    private int _ActionListStart = 4, _ActionListEnd = 26;
    private int _Vision = 1;
    private string AiNumber;
    internal Biology Parent;
    internal float Vision;
    internal List<Biology> Visible_Ally_Biologys;
    internal List<Biology> Visible_Foe_Biologys;
    public BiologyAI(Biology Parent, string AiNumber)
    {
        //如果無此資料
        if (AiNumber == "" || GameDB.Instance.BiologyAi.ContainsKey(AiNumber) == false) return;

        this.AiNumber = AiNumber;
        this.Parent = Parent;
        ConditionList = GetConditionList();
        ActionList = GetActionList();
        Vision = GetVision();

    }
    private List<string> GetConditionList()
    {
        List<string> n = new List<string>();
        for (var i = _ConditionListStart; i < _ConditionListEnd; i += 2)
        {
            n.Add(GameDB.Instance.BiologyAi[AiNumber][i]);
        }
        return n;
    }
    private List<string> GetActionList()
    {
        List<string> n = new List<string>();
        for (var i = _ActionListStart; i < _ActionListEnd; i += 2)
        {
            n.Add(GameDB.Instance.BiologyAi[AiNumber][i]);
        }
        return n;
    }
    private float GetVision()
    {
        return float.Parse(GameDB.Instance.BiologyAi[AiNumber][_Vision]);
    }

    public void Update()
    {
        VisibleBioUpdate(); //取得視線範圍內的生物
        BioConditionUpdate();//狀況檢查並取得行為與目標

        //執行行為
    }

    private void BioConditionUpdate()
    {
        BiologyAI_Condition.Instance.Condition(this);
    }


    private void VisibleBioUpdate() //fixme:之後需要效能優化，不需要每一楨都執行  
    {
        List<Biology> Visible_Ally_Biologys = new List<Biology>();
        List<Biology> Visible_Foe_Biologys = new List<Biology>();
        foreach (var t in Parent.Biologys)
        {
            //fixme: 超醜
            if (Vector3.Distance(Parent.transform.position, t.transform.position) > Vision) continue;

            if (Parent.Type == uFantasy.Enum.BiologyType.Player)
            {
                if (t.Type == uFantasy.Enum.BiologyType.Item || t.Type == uFantasy.Enum.BiologyType.Npc) continue;
                if (t.Type == uFantasy.Enum.BiologyType.Player)
                {
                    Visible_Ally_Biologys.Add(t.gameObject.GetComponent<Biology>());
                }
                else
                {
                    Visible_Foe_Biologys.Add(t.gameObject.GetComponent<Biology>());
                }
            }
            else
            {
                if (t.Type == uFantasy.Enum.BiologyType.Item || t.Type == uFantasy.Enum.BiologyType.Npc) continue;
                if (t.Type == uFantasy.Enum.BiologyType.Player)
                {
                    Visible_Foe_Biologys.Add(t.gameObject.GetComponent<Biology>());
                }
                else
                {
                    Visible_Ally_Biologys.Add(t.gameObject.GetComponent<Biology>());
                }
            }
        }
        this.Visible_Ally_Biologys = Visible_Ally_Biologys;
        this.Visible_Foe_Biologys = Visible_Foe_Biologys;
    }
}