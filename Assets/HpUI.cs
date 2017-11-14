using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    UnityEngine.UI.Image HPGreen, HPRed, HPWhite;
    private Biology Biology;
    private BiologyAttr biologyAttr;
    private float HPRed_t, HPWhite_t = 0;
    // Use this for initialization
    private void Awake()
    {
        HPGreen = transform.Find("HPGreen").GetComponent<UnityEngine.UI.Image>();
        HPRed = transform.Find("HPRed").GetComponent<UnityEngine.UI.Image>();
        HPWhite = transform.Find("HPGreen/HPWhite").GetComponent<UnityEngine.UI.Image>();

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
            HPRed.fillAmount = EasingFunction.EaseInExpo(HPRed.fillAmount, HPGreen.fillAmount, HPRed_t += Time.deltaTime / 0.25f);
        }
        else
        {
            HPRed.fillAmount = HPGreen.fillAmount;
        }
    }
    private void HPWhiteAnimation()
    {
        if (1 - HPGreen.fillAmount < HPWhite.fillAmount)
        {
            HPWhite.fillAmount = EasingFunction.EaseInExpo(HPWhite.fillAmount, 1 - HPGreen.fillAmount, HPWhite_t += Time.deltaTime / 0.25f);
        }
        else
        {
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
    }

    internal void Hide()
    {
        gameObject.SetActive(false);
    }
}
