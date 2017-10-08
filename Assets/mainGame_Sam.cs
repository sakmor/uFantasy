using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainGame_Sam : MonoBehaviour
{
    private Biology Leader;

    // Use this for initialization
    private void Start()
    {
        Leader = GameObject.Find("10001 騎士01").GetComponent<Biology>();
        GameObject n = new GameObject("navMesh");
        // n.AddComponent<MeshFilter>().mesh = UnityEngine.AI.NavMeshBuildSourceShape.Mesh.;

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
            GameObject.Find("Line").GetComponent<LineRenderer>().enabled = false;
            Leader.BiologyMovement.NavMeshAgent.velocity *= 0.5f;
            Leader.BiologyMovement.Stop();
        }
        //  Mouse左鍵
        if (Input.GetMouseButton(0) == false)
            return;

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
}
