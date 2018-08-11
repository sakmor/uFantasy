using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyLook : MonoBehaviour
{
    [SerializeField] internal float WalkStep;
    private float HitStopTime;
    [SerializeField] internal Animator Animator;
    [SerializeField] internal Biology Biology;
    [SerializeField] internal SkinnedMeshRenderer SkinnedMeshRenderer;
    [SerializeField] internal Transform M1;
    [SerializeField] internal List<Material> Materials;
    // Use this for initialization
    private void Start()
    {
        HitStopTime = 0.15f;
        Materials = new List<Material>(SkinnedMeshRenderer.materials);
        Biology = GetComponent<Biology>();
    }
    internal void StartHitStop()
    {
        if (Animator.enabled == false) return;
        Animator.enabled = false;
        StartCoroutine("RestLookAfterHitStopTime");
    }
    internal void StartHitFlash()
    {
        if (Animator.enabled == false) return;
        SetMaterialsColor(Color.gray);
        StartCoroutine("RestLookAfterHitStopTime");
    }

    IEnumerator RestLookAfterHitStopTime()
    {
        float waitCounter = 0;
        while (waitCounter < HitStopTime) { waitCounter += Time.deltaTime; yield return null; }
        RestLook();
    }
    private void RestLook()
    {
        Animator.enabled = true;
        SetMaterialsColor(Color.black);
    }


    private void SetMaterialsColor(Color color)
    {
        foreach (var m in Materials)
        {
            m.SetColor("_Color", color);
        }
    }
}
