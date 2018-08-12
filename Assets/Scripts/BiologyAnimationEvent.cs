using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyAnimationEvent : MonoBehaviour
{
    public HpUI HpUI;
    public Biology Biology;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void HideHPUI()
    {
        HpUI.FadeOut();
    }

    internal void Hit()
    {
        if (Biology.Target == null) return;
        Biology.Target.GetDamage(Biology.BiologyAttr.Atk);
        Biology.BiologyLook.StartHitStop();
        Biology.Target.BiologyLook.StartHitFlash();
        Biology.Target.BiologyLook.StartHitShake();
        Biology.Target.BiologyLook.StartHitStop();
    }
}
