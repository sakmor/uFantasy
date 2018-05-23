using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class myDropdown : MonoBehaviour
{
    public Dropdown Dropdown;
    public float[] DropdownValue = { 1, 0.75f, 0.5f };
    private int ScreenWidth;
    private int ScreenHeight;
    private UnityEngine.PostProcessing.PostProcessingBehaviour PostProcessingBehaviour;
    void Start()
    {
        PostProcessingBehaviour = Camera.main.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        Dropdown.onValueChanged.AddListener(delegate
        {
            myDropdownValueChangedHandler(Dropdown);
        });
    }
    void Destroy()
    {
        Dropdown.onValueChanged.RemoveAllListeners();
    }

    private void myDropdownValueChangedHandler(Dropdown target)
    {
        float s = DropdownValue[target.value];
        Screen.SetResolution(Mathf.FloorToInt(ScreenWidth * s), Mathf.FloorToInt(ScreenHeight * s), true);//fixme:應該在整個遊戲的進入點
    }

    public void SetDropdownIndex(int index)
    {
        Dropdown.value = index;
    }
    public void ToggleSSAO()
    {
        PostProcessingBehaviour.profile.ambientOcclusion.enabled = !PostProcessingBehaviour.profile.ambientOcclusion.enabled;
    }
    public void TogglePcControl()
    {
        mainGame_Sam mainGame_Sam = GameObject.Find("mainGame").GetComponent<mainGame_Sam>();
        if (mainGame_Sam.IsPcControl == true) mainGame_Sam.SetPcControl();
        if (mainGame_Sam.IsPcControl == false) mainGame_Sam.SetMobileControl();

    }
}
