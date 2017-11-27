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
    private MeshCollider MeshCollider;
    private BoxCollider BoxCollider;
    public visualJoyStick visualJoyStick;
    public Canvas Canvas;
    public HighlightsFX HighlightsFX;

    public List<Biology> SelectBiologys = new List<Biology>();

    public List<Renderer> SelectBiologysRenderer = new List<Renderer>();
    // Use this for initialization
    void Start()
    {
        SelectBoxTransform = transform.Find("SelectBox");
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
        PerspectiveInitialize();
        OrthographicInitialize();
        GetSelectBoxMesh();

    }

    private void GetSelectBoxMesh()
    {
        SelectBoxMesh = Instantiate(SelectBoxTransform.GetComponent<MeshFilter>().sharedMesh);
        SelectBoxVerts = SelectBoxMesh.vertices;
        _SelectBoxVerts = SelectBoxMesh.vertices;
        SelectBoxMesh.MarkDynamic();
    }

    private void OrthographicInitialize()
    {
        if (Camera.main.orthographic == false) return;
        SelectBoxRoateUpdate();
        AddBoxCollider();
    }

    private void AddBoxCollider()
    {
        if (SelectBoxTransform.GetComponent<MeshCollider>()) DestroyImmediate(SelectBoxTransform.GetComponent<MeshCollider>());
        BoxCollider = SelectBoxTransform.gameObject.AddComponent<BoxCollider>();
        BoxCollider.isTrigger = true;
    }

    private void PerspectiveInitialize()
    {
        if (Camera.main.orthographic == true) return;
        AddMeshCollider();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsNotDraw()) return;
        DrawFrame();
        DrawBox();
    }


    private void DrawBox()
    {
        OrthographicDrawBox();
        PerspectiveDrawBox();
    }

    private void DrawBoxInitialize()
    {
        SelectBoxMesh.vertices = _SelectBoxVerts;
        // MeshCollider.sharedMesh = SelectBoxMesh;
    }

    private void PerspectiveDrawBox()
    {
        if (Camera.main.orthographic == true) return;
        DrawBoxInitialize();
        ResizeSelectBoxMesh();
        MeshCollider.sharedMesh = SelectBoxMesh;
    }

    private void AddMeshCollider()
    {
        if (SelectBoxTransform.GetComponent<BoxCollider>()) DestroyImmediate(SelectBoxTransform.GetComponent<BoxCollider>());
        MeshCollider = SelectBoxTransform.gameObject.AddComponent<MeshCollider>();
        MeshCollider.convex = true;
        MeshCollider.isTrigger = true;
    }

    private void OrthographicDrawBox()
    {
        if (Camera.main.orthographic == false) return;
        DrawBoxInitialize();
        ResizeSelectBoxMesh();
        SelectBoxMesh.RecalculateBounds();
        BoxCollider.center = SelectBoxMesh.bounds.center;
        BoxCollider.size = SelectBoxMesh.bounds.size;
    }

    public void SelectBoxRoateUpdate()
    {
        SelectBoxTransform.rotation = Camera.main.transform.rotation;
    }

    private void ResizeSelectBoxMesh()
    {
        Ray sRay = Camera.main.ScreenPointToRay(SelectFrameTransform.position);
        Ray eRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (IsTinyDrawRange()) return;

        SelectBoxVerts[0] = SelectBoxVerts[8] = SelectBoxVerts[23] = SelectBoxTransform.InverseTransformPoint(sRay.origin);
        SelectBoxVerts[3] = SelectBoxVerts[9] = SelectBoxVerts[12] = SelectBoxTransform.InverseTransformPoint(sRay.origin + sRay.direction * 100);

        SelectBoxVerts[7] = SelectBoxVerts[18] = SelectBoxVerts[21] = SelectBoxTransform.InverseTransformPoint(eRay.origin);
        SelectBoxVerts[6] = SelectBoxVerts[14] = SelectBoxVerts[19] = SelectBoxTransform.InverseTransformPoint(eRay.origin + eRay.direction * 100);

        Ray tRay1 = Camera.main.ScreenPointToRay(new Vector3(SelectFrameTransform.position.x, Input.mousePosition.y, SelectFrameTransform.position.z));
        SelectBoxVerts[1] = SelectBoxVerts[17] = SelectBoxVerts[22] = SelectBoxTransform.InverseTransformPoint(tRay1.origin);
        SelectBoxVerts[2] = SelectBoxVerts[13] = SelectBoxVerts[16] = SelectBoxTransform.InverseTransformPoint(tRay1.origin + tRay1.direction * 100);

        Ray tRay2 = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, SelectFrameTransform.position.y, SelectFrameTransform.position.z));
        SelectBoxVerts[4] = SelectBoxVerts[11] = SelectBoxVerts[20] = SelectBoxTransform.InverseTransformPoint(tRay2.origin);
        SelectBoxVerts[5] = SelectBoxVerts[10] = SelectBoxVerts[15] = SelectBoxTransform.InverseTransformPoint(tRay2.origin + tRay2.direction * 100);
        SelectBoxMesh.vertices = SelectBoxVerts;
    }
    private bool IsTinyDrawRange()
    {

        if (Mathf.Abs(SelectFrameTransform.position.x - Input.mousePosition.x) < 0.1f) return true;
        if (Mathf.Abs(SelectFrameTransform.position.y - Input.mousePosition.y) < 0.1f) return true;
        return false;
    }
    private bool IsNotDraw()
    {
        if (EventSystem.current.currentSelectedGameObject == visualJoyStick.gameObject)
        {
            Rest();
            Hide();
            return true;
        }

        if (Input.GetMouseButton(0) == false || EventSystem.current.IsPointerOverGameObject() && !IsStart)
        {
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

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Biology>() == null) return;
        Biology b = other.GetComponent<Biology>();
        Renderer r = b.transform.Find("Model/Model").GetComponent<Renderer>();
        SelectBiologys.Add(b);
        SelectBiologysRenderer.Add(r);
        HighlightsFXUpdate();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Biology>() == null) return;
        Biology b = other.GetComponent<Biology>();
        Renderer r = b.transform.Find("Model/Model").GetComponent<Renderer>();
        SelectBiologys.Remove(b);
        SelectBiologysRenderer.Remove(r);
        HighlightsFXUpdate();

    }
    void HighlightsFXUpdate()
    {
        HighlightsFX.ClearOutlineData();
        HighlightsFX.AddRenderers(SelectBiologysRenderer);
    }
}
