using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mainGame_Sam : MonoBehaviour
{
    public float ResolutionScale = 0.5f;
    public Biology Leader; //fixme:暫時改為public的
    public Biology[] Biologys;
    private DotLine DotLine;
    private HighlightsFX HighlightsFX;

    private void Awake()
    {
        Biologys = (Biology[])FindObjectsOfType(typeof(Biology));
    }
    // Use this for initialization
    private void Start()
    {
        Leader = GameObject.Find("10001 騎士01").GetComponent<Biology>(); //fixme:暫時指定隊長
        DotLine = GameObject.Find("Line").GetComponent<DotLine>();
        HighlightsFX = Camera.main.GetComponent<HighlightsFX>();
    }
    void HighlightsFXAdd(GameObject n)
    {
        HighlightsFX.ClearOutlineData();

        Renderer[] Renderer = n.GetComponentsInChildren<Renderer>();

        List<Renderer> one = new List<Renderer>(Renderer);

        HighlightsFX.AddRenderers(one);
    }
    // Update is called once per frame
    private void Update()
    {

        InputProcess();

    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void InputProcess()
    {
        //當玩家點選到UI物件時跳出
        if (IsPointerOverUIObject()) return;

        if (GetInput() == false)
        {
            DotLine.DrawLineStop();
            return;
        }

        //由攝影機產生一條射線
        Ray ray = Camera.main.ScreenPointToRay(GetInputPostion());
        RaycastHit[] hits = Physics.RaycastAll(ray);

        // // 走訪每一個被Hit到的GameObject
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.GetComponent<Biology>())
            {
                HighlightsFXAdd(Leader.gameObject);
                Leader.BiologyMovement.Stop();
                Leader = hit.transform.GetComponent<Biology>();
                return;
            }
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
