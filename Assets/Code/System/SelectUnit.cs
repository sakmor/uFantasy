using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectUnit : MonoBehaviour
{
    private bool IsStart;
    public Image SelectFrameImage { get; private set; }
    private RectTransform SelectFrameRectTransform;
    private Mesh SelectBoxMesh;
    private Transform SelectBoxTransform, SelectFrameTransform;
    private Vector3[] SelectBoxVerts;
    private Vector3[] _SelectBoxVerts;
    public Canvas Canvas;
    private bool n;


    // Use this for initialization
    void Start()
    {
        SelectBoxInitialize();
        SelectFrameInitialize();
    }

    private void SelectFrameInitialize()
    {
        SelectFrameTransform = transform.Find("SelectFrame");
        SelectFrameTransform.SetParent(Canvas.transform);
        SelectFrameImage = SelectFrameTransform.GetComponent<UnityEngine.UI.Image>();
        SelectFrameRectTransform = SelectFrameTransform.GetComponent<RectTransform>();
    }

    private void SelectBoxInitialize()
    {
        SelectBoxTransform = transform.Find("SelectBox");
        SelectBoxMesh = Instantiate(SelectBoxTransform.GetComponent<MeshFilter>().sharedMesh);
        SelectBoxVerts = SelectBoxMesh.vertices;
        _SelectBoxVerts = SelectBoxMesh.vertices;
        SelectBoxMesh.MarkDynamic();
    }

    // Update is called once per frame
    void Update()
    {

        if (IsNotDraw()) return;
        DrawFrame();
        DrawBox();
        SelectBio();

        n = !n;
        // SelectBoxTransform.GetComponent<MeshCollider>().enabled = n;

    }


    private void DrawBox()
    {
        SelectBoxMesh.vertices = _SelectBoxVerts;

        SelectBoxTransform.GetComponent<MeshCollider>().sharedMesh = SelectBoxMesh;
        if (Mathf.Abs(SelectFrameTransform.position.x - Input.mousePosition.x) <= 0.1f) return;
        if (Mathf.Abs(SelectFrameTransform.position.y - Input.mousePosition.y) <= 0.1f) return;

        Ray sRay = Camera.main.ScreenPointToRay(SelectFrameTransform.position);
        SelectBoxVerts[0] = SelectBoxVerts[8] = SelectBoxVerts[23] = SelectBoxTransform.InverseTransformPoint(sRay.origin);
        SelectBoxVerts[3] = SelectBoxVerts[9] = SelectBoxVerts[12] = SelectBoxTransform.InverseTransformPoint(sRay.origin + sRay.direction * Camera.main.farClipPlane);


        Ray eRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        SelectBoxVerts[7] = SelectBoxVerts[18] = SelectBoxVerts[21] = SelectBoxTransform.InverseTransformPoint(eRay.origin);
        SelectBoxVerts[6] = SelectBoxVerts[14] = SelectBoxVerts[19] = SelectBoxTransform.InverseTransformPoint(eRay.origin + eRay.direction * Camera.main.farClipPlane);


        Ray tRay1 = Camera.main.ScreenPointToRay(new Vector3(SelectFrameTransform.position.x, Input.mousePosition.y, SelectFrameTransform.position.z));
        SelectBoxVerts[1] = SelectBoxVerts[17] = SelectBoxVerts[22] = SelectBoxTransform.InverseTransformPoint(tRay1.origin);
        SelectBoxVerts[2] = SelectBoxVerts[13] = SelectBoxVerts[16] = SelectBoxTransform.InverseTransformPoint(tRay1.origin + tRay1.direction * Camera.main.farClipPlane);

        Ray tRay2 = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, SelectFrameTransform.position.y, SelectFrameTransform.position.z));
        SelectBoxVerts[4] = SelectBoxVerts[11] = SelectBoxVerts[20] = SelectBoxTransform.InverseTransformPoint(tRay2.origin);
        SelectBoxVerts[5] = SelectBoxVerts[10] = SelectBoxVerts[15] = SelectBoxTransform.InverseTransformPoint(tRay2.origin + tRay2.direction * Camera.main.farClipPlane);


        SelectBoxMesh.vertices = SelectBoxVerts;
        Debug.Log(Time.time + "===========");
        foreach (var item in SelectBoxVerts)
        {
            Debug.Log(Time.time + "  " + item);
        }

        SelectBoxTransform.GetComponent<MeshCollider>().sharedMesh = SelectBoxMesh;
    }

    private void SelectBio()
    {

    }

    private bool IsNotDraw()
    {

        if (Input.GetMouseButton(0) == false || EventSystem.current.IsPointerOverGameObject() && !IsStart)
        {
            n = false;
            Rest();
            Hide();
            return true;
        }
        return false;
    }

    private void Rest()
    {
        IsStart = false;
        SelectFrameRectTransform.sizeDelta = Vector2.zero;

    }

    private void Hide()
    {
        SelectFrameImage.enabled = false;
    }

    private void Show()
    {
        SelectFrameImage.enabled = true;
    }

    private void DrawFrame()
    {
        if (IsStart == false)
        {
            SelectFrameTransform.position = Input.mousePosition;
            IsStart = true;
            Show();

            return;
        }

        float _x = (Input.mousePosition.x - SelectFrameTransform.position.x);
        float _y = (SelectFrameTransform.position.y - Input.mousePosition.y);
        SelectFrameRectTransform.pivot = new Vector2(_x < 0 ? 1 : 0, _y > 0 ? 1 : 0);
        ResizeRect();

    }

    private void ResizeRect()
    {
        SelectFrameRectTransform.sizeDelta = new Vector2(Mathf.Abs(Input.mousePosition.x - SelectFrameTransform.position.x), Mathf.Abs(SelectFrameTransform.position.y - Input.mousePosition.y));
    }

}
