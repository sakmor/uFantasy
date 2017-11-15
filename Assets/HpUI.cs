using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    UnityEngine.UI.Image HPGreen, HPRed, HPWhite, HPHeader;
    private Biology Biology;
    private BiologyAttr biologyAttr;
    private float HPRed_t, HPWhite_t, HPGreenWidth;
    // Use this for initialization
    private void Awake()
    {
        HPGreen = transform.Find("HPValue").GetComponent<UnityEngine.UI.Image>();
        HPRed = transform.Find("HPWhite").GetComponent<UnityEngine.UI.Image>();
        HPHeader = transform.Find("HPHeader").GetComponent<UnityEngine.UI.Image>();
        HPWhite = transform.Find("HPValue/vHPWhite").GetComponent<UnityEngine.UI.Image>();
        HPGreenWidth = HPGreen.rectTransform.sizeDelta.x;
    }

    // Update is called once per frame
    private void Update()
    {
        ChangePos();

        HPRedAnimation();
        HPWhiteAnimation();
    }

    private void HPRedAnimation()
    {
        if (HPGreen.fillAmount < HPRed.fillAmount)
        {
            ShowHPHeader();
            HPRed.fillAmount = EasingFunction.EaseInExpo(HPRed.fillAmount, HPGreen.fillAmount, HPRed_t += Time.deltaTime / 0.25f);
        }
        else
        {
            HideHPHeader();
            HPRed.fillAmount = HPGreen.fillAmount;
        }
    }
    private void HPWhiteAnimation()
    {
        if (1 - HPGreen.fillAmount < HPWhite.fillAmount)
        {
            ShowHPHeader();
            HPWhite.fillAmount = EasingFunction.EaseInExpo(HPWhite.fillAmount, 1 - HPGreen.fillAmount, HPWhite_t += Time.deltaTime / 0.25f);
        }
        else
        {
            HideHPHeader();
            HPWhite.fillAmount = 1 - HPGreen.fillAmount;
        }
    }
    private void ChangePos()
    {
        transform.position = Camera.main.WorldToScreenPoint(Biology.transform.position + Vector3.up * 2f);//fixme:應該抓綁點
    }

    internal void SetBio(Biology Biology)
    {
        this.Biology = Biology;
        biologyAttr = Biology.BiologyAttr;
    }

    public void ChangeHP()
    {
        HPRed_t = HPWhite_t = 0;
        HPGreen.fillAmount = (float)biologyAttr.Hp / (float)biologyAttr.HpMax;
        ChangeHPHeader();
    }

    internal void ChangeHPHeader()
    {
        float posX = HPGreenWidth * (float)HPGreen.fillAmount;
        HPHeader.rectTransform.anchoredPosition = new Vector2(posX, 0);
    }

    internal void Hide()
    {
        gameObject.SetActive(false);
    }
    internal void HideHPHeader()
    {
        HPHeader.enabled = false;
    }
    internal void ShowHPHeader()
    {
        HPHeader.enabled = true;
    }
}
