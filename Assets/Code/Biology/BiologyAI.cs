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
    private string AiNumber;
    private Biology Parent;
    private List<Biology> Visible_Ally_Biologys;
    private List<Biology> Visible_Foe_Biologys;
    public BiologyAI(Biology Parent, string AiNumber)
    {
        //如果無此資料
        if (AiNumber == "" || GameDB.Instance.biologyAi.ContainsKey(AiNumber) == false) return;

        this.AiNumber = AiNumber;
        this.Parent = Parent;
        ConditionList = GetConditionList();
        ActionList = GetActionList();

    }
    private List<string> GetConditionList()
    {
        List<string> n = new List<string>();
        for (var i = _ConditionListStart; i < _ConditionListEnd; i += 2)
        {
            n.Add(GameDB.Instance.biologyAi[AiNumber][i]);
        }
        return n;
    }
    private List<string> GetActionList()
    {
        List<string> n = new List<string>();
        for (var i = _ActionListStart; i < _ActionListEnd; i += 2)
        {
            n.Add(GameDB.Instance.biologyAi[AiNumber][i]);
        }
        return n;
    }

    public void Update()
    {
        VisibleBioUpdate();
        ConditionBioUpdate();
    }

    private void ConditionBioUpdate()
    {
        BiologyAI_Condition.Instance.Condition(this, "Ally:HP < 90%");
    }

    private void VisibleBioUpdate() //fixme:之後需要效能優化，不需要每一楨都執行  
    {
        List<Biology> Visible_Ally_Biologys = new List<Biology>();
        List<Biology> Visible_Foe_Biologys = new List<Biology>();
        foreach (var t in Parent.Biologys)
        {
            //fixme: 超醜
            if (Vector3.Distance(Parent.transform.position, t.transform.position) > 10) continue;//fixme:應該索引AI表

            if (Parent.Type == uFantasy.Enum.BiologyType.Player)
            {
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
                if (t.Type == uFantasy.Enum.BiologyType.Player)
                {
                    Visible_Foe_Biologys.Add(t.gameObject.GetComponent<Biology>());
                }
                else
                {
                    Visible_Ally_Biologys.Add(t.gameObject.GetComponent<Biology>());
                }
            }
            this.Visible_Ally_Biologys = Visible_Ally_Biologys;
            this.Visible_Foe_Biologys = Visible_Foe_Biologys;
        }
    }
}