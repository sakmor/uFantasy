using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyLook : MonoBehaviour
{
    [SerializeField] internal float WalkStep, HitStopTime;
    [SerializeField] internal Animator Animator;
    [SerializeField] internal Biology Biology;
    [SerializeField] internal SkinnedMeshRenderer SkinnedMeshRenderer;
    [SerializeField] internal Transform M1;
    [SerializeField] internal List<Material> Materials;
    [SerializeField] private float flashLight = 0.015f, _flashLight;
    // Use this for initialization
    private void Start()
    {
        Materials = new List<Material>(SkinnedMeshRenderer.materials);
        Biology = GetComponent<Biology>();
    }

    internal void StartHitStop()
    {
        StartCoroutine("HitStop");
    }

    // Update is called once per frame
    private void Update()
    {
        if (_flashLight <= 0.0f) return;
        _flashLight -= 0.025f;
        UpdateMaterialsColor();

    }

    internal void HitFlash()
    {
        _flashLight = 0.5f;
    }

    internal void UpdateMaterialsColor() //fixme：閃光系統隨便寫的，之後要修正
    {
        foreach (Material t in Materials)
        {
            t.SetColor("_Color", new Color(_flashLight, _flashLight, _flashLight, 1));
        }
    }

    IEnumerator HitFLash()
    {
        //initialize
        float waitCounter = 0;

        Color _Color = Materials[0].GetColor("_EmissionColor");
        foreach (var m in Materials) { m.SetColor("_EmissionColor", _Color + 0.65f * Color.white); }

        //Wait 
        while (waitCounter < HitStopTime) { waitCounter += Time.deltaTime; yield return null; }

        //End
        foreach (var m in Materials) { m.SetColor("_EmissionColor", _Color); }
    }

    IEnumerator HitStop()
    {
        //initialize
        float waitCounter = 0;

        Vector3 _postion = Biology.transform.position;
        Biology.Animator.enabled = false;

        //Wait 
        while (waitCounter < HitStopTime) { waitCounter += Time.deltaTime; yield return null; }

        //End
        Biology.Animator.enabled = true;
    }
}
