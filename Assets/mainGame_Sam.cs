using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainGame_Sam : MonoBehaviour
{
    private Biology Leader;
    LineRenderer LineRenderer;


    // Use this for initialization
    private void Start()
    {
        Leader = GameObject.Find("10001 騎士01").GetComponent<Biology>(); //fixme:暫時指定隊長
        LineRenderer = GameObject.Find("Line").GetComponent<LineRenderer>();
        GameObject n = new GameObject("navMesh");

    }

    // Update is called once per frame
    private void Update()
    {

        InputProcess();

    }

    private void InputProcess()
    {
        if (Input.GetMouseButtonUp(0) == true)
        {
            LineRenderer.enabled = false;
            Leader.BiologyMovement.NavMeshAgent.velocity *= 0.5f;
            Leader.BiologyMovement.Stop();
        }
        //  Mouse左鍵
        if (Input.GetMouseButton(0) == false)
            return;

        // DrawLine
        LineRenderer.enabled = true;
        LineRenderer.positionCount = Leader.BiologyMovement.NavMeshAgent.path.corners.Length;
        LineRenderer.SetPositions(Leader.BiologyMovement.NavMeshAgent.path.corners);
        float lineLength = GetLineLenght();
        LineRenderer.material.SetFloat("_RepeatCount", lineLength * 2);


        //由攝影機產生一條射線
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        // // 走訪每一個被Hit到的GameObject
        foreach (RaycastHit hit in hits)
        {
            if (hit.normal != new Vector3(0, 1, 0)) return;
            Leader.BiologyMovement.MoveTo(hit.point);
            GameObject.Find("Select_Arrow").transform.position = hit.point; //fixme: 不要搜尋名稱的方式
        }


    }

    private float GetLineLenght()
    {
        float dist = 0;
        for (var i = 0; i < LineRenderer.positionCount; i++)
        {
            int next_i = i == LineRenderer.positionCount - 1 ? i : i + 1;
            dist += Vector3.Distance(Leader.BiologyMovement.NavMeshAgent.path.corners[i], Leader.BiologyMovement.NavMeshAgent.path.corners[next_i]);
        }
        return dist;
    }
}
