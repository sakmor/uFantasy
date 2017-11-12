using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    float _HP;
    UnityEngine.UI.Image Image;
    private Biology Biology;
    private BiologyAttr biologyAttr;
    // Use this for initialization
    private void Start()
    {
        Image = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    internal void SetBio(Biology Biology)
    {
        this.Biology = Biology;
        biologyAttr = Biology.BiologyAttr;

    }

    public void _Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(Biology.transform.position + Vector3.up * 2f);//fixme:應該抓綁點

        if (biologyAttr.Hp == _HP) return;//如果資料沒有變動則跳出
        _HP = biologyAttr.Hp;
        Image.fillAmount = (float)biologyAttr.Hp / (float)biologyAttr.HpMax;
    }
}
