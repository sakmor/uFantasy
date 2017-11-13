using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    float _HPCurrentFillAmount;
    UnityEngine.UI.Image Image, HPCurrent;
    private Biology Biology;
    private BiologyAttr biologyAttr;
    private float t = 0;
    // Use this for initialization
    private void Awake()
    {
        Image = GetComponent<UnityEngine.UI.Image>();
        HPCurrent = transform.Find("HPCurrent").GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(Biology.transform.position + Vector3.up * 2f);//fixme:應該抓綁點

        if (HPCurrent.fillAmount != _HPCurrentFillAmount) { t = 0; _HPCurrentFillAmount = HPCurrent.fillAmount; }
        if (HPCurrent.fillAmount >= Image.fillAmount) { Image.fillAmount = HPCurrent.fillAmount; return; }
        Image.fillAmount = EasingFunction.EaseInExpo(Image.fillAmount, HPCurrent.fillAmount, t += Time.deltaTime / 0.25f);
    }

    internal void SetBio(Biology Biology)
    {
        this.Biology = Biology;
        biologyAttr = Biology.BiologyAttr;
    }

    public void ChangeHP()
    {
        t = 0;
        HPCurrent.fillAmount = (float)biologyAttr.Hp / (float)biologyAttr.HpMax;

    }

    internal void Hide()
    {
        gameObject.SetActive(false);
    }
}
