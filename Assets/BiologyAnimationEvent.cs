using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyAnimationEvent : MonoBehaviour
{
    public HpUI HpUI;

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
}
