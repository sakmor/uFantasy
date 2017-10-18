﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainGame_Sam : MonoBehaviour
{
    private Biology Leader;
    private DotLine DotLine;


    // Use this for initialization
    private void Start()
    {
        Screen.SetResolution(Mathf.FloorToInt(Screen.width * 1f), Mathf.FloorToInt(Screen.height * 1f), true);//fixme:應該在整個遊戲的進入點
        Leader = GameObject.Find("10001 騎士01").GetComponent<Biology>(); //fixme:暫時指定隊長
        DotLine = GameObject.Find("Line").GetComponent<DotLine>();
        GameObject n = new GameObject("navMesh");

    }

    // Update is called once per frame
    private void Update()
    {

        InputProcess();

    }

    private void InputProcess()
    {
        if (GetInput() == false)
        {
            DotLine.DrawLineStop();
            Leader.BiologyMovement.NavMeshAgent.velocity *= 0.5f;
            Leader.BiologyMovement.Stop();
            return;
        }

        //由攝影機產生一條射線
        Ray ray = Camera.main.ScreenPointToRay(GetInputPostion());
        RaycastHit[] hits = Physics.RaycastAll(ray);

        // // 走訪每一個被Hit到的GameObject
        foreach (RaycastHit hit in hits)
        {
            if (Leader.BiologyMovement.MoveTo(hit.point))
                DotLine.DrawLine(Leader.BiologyMovement.NavMeshAgent.path.corners);

        }

    }

    private bool GetInput()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) if (Input.GetMouseButton(0)) return true;
        if (Application.platform == RuntimePlatform.Android) if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)) return true;

        return false;
    }

    private Vector3 GetInputPostion()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text = Input.mousePosition.ToString("F0");
            return Input.mousePosition;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text = Input.GetTouch(0).position.ToString("F0");
            return Input.GetTouch(0).position;
        }


        return Vector3.zero;
    }

}
