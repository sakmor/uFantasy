using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyLook : MonoBehaviour
{
    [SerializeField] internal float WalkStep;
    [SerializeField] internal Animator Animator;
    [SerializeField] internal SkinnedMeshRenderer SkinnedMeshRenderer;
    [SerializeField] internal Transform M1;
    [SerializeField] private List<Material> Materials;
    [SerializeField] private float flashLight = 0.015f, _flashLight;
    // Use this for initialization
    private void Start()
    {
        Materials = new List<Material>(SkinnedMeshRenderer.materials);
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
}
