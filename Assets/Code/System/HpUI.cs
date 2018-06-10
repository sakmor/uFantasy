using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    [SerializeField] internal UnityEngine.UI.Image HPValue, HPRed, HPWhite, HPHeader;
    private Biology Biology;
    private BiologyAttr biologyAttr;
    private float HPRed_t, HPWhite_t, HPValueWidth;
    private Transform M1;

    // Use this for initialization
    private void Awake()
    {
        HPValueWidth = HPValue.rectTransform.sizeDelta.x;

    }
    private void Start()
    {
        M1 = Biology.transform.Find("Model/M1");
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
        if (HPValue.fillAmount < HPRed.fillAmount)
        {
            ShowHPHeader();
            HideHPWhite();
            ShowHPRed();
            HPRed.fillAmount = EasingFunction.EaseInExpo(HPRed.fillAmount, HPValue.fillAmount, HPRed_t += Time.deltaTime / 0.25f);
        }
        else
        {
            HideHPHeader();
            HPRed.fillAmount = HPValue.fillAmount;
        }
    }
    private void HPWhiteAnimation()
    {
        if (1 - HPValue.fillAmount < HPWhite.fillAmount)
        {
            ShowHPHeader();
            HideHPRed();
            ShowHPWhite();
            HPWhite.fillAmount = EasingFunction.EaseInExpo(HPWhite.fillAmount, 1 - HPValue.fillAmount, HPWhite_t += Time.deltaTime / 0.25f);
        }
        else
        {
            HideHPHeader();
            HPWhite.fillAmount = 1 - HPValue.fillAmount;
        }
    }
    private void ChangePos()
    {
        transform.position = Camera.main.WorldToScreenPoint(M1.position);//fixme:應該抓綁點
    }

    internal void SetBio(Biology Biology)
    {
        this.Biology = Biology;
        biologyAttr = Biology.BiologyAttr;
    }

    public void ChangeHP()
    {
        HPRed_t = HPWhite_t = 0;
        HPValue.fillAmount = (float)biologyAttr.Hp / (float)biologyAttr.HpMax;
        ChangeHPHeader();
    }

    internal void ChangeHPHeader()
    {
        float posX = HPValueWidth * (float)HPValue.fillAmount;
        HPHeader.rectTransform.anchoredPosition = new Vector2(posX, 0);
    }

    internal void FadeOut()
    {
        GetComponent<Animator>().Play("UIFadeOut");
        // gameObject.SetActive(false);
    }

    internal void Show()
    {
        gameObject.SetActive(true);
    }
    internal void HideHPHeader()
    {
        HPHeader.enabled = false;
    }
    internal void ShowHPHeader()
    {
        HPHeader.enabled = true;
    }

    internal void HideHPRed()
    {
        HPRed.enabled = false;
    }
    internal void ShowHPRed()
    {
        HPRed.enabled = true;
    }

    internal void HideHPWhite()
    {
        HPWhite.enabled = false;
    }
    internal void ShowHPWhite()
    {
        HPWhite.enabled = true;
    }
}
