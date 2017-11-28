using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectUnit : MonoBehaviour
{
    private Vector2 Input;
    private bool IsStart;
    public Image SelectFrameImage { get; private set; }
    private RectTransform SelectFrameRectTransform;
    private Mesh SelectBoxMesh;
    private Transform SelectBoxTransform, SelectFrameTransform;
    private Vector3[] SelectBoxVerts;
    private List<Vector3> _SelectBoxVerts;
    private MeshCollider MeshCollider;
    private BoxCollider BoxCollider;
    public visualJoyStick visualJoyStick;
    public Canvas Canvas;
    private HighlightsFX HighlightsFX;

    public List<Biology> SelectBiologys = new List<Biology>();

    private List<Renderer> SelectBiologysRenderer = new List<Renderer>();

    // Use this for initialization
    void Start()
    {
        SelectBoxTransform = transform.Find("SelectBox");
        HighlightsFX = Camera.main.GetComponent<HighlightsFX>();
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

    internal void ButtonDown(Vector2 pos)
    {
        if (IsSelectOtherUI()) return;
        Input = pos;
        Click();
        Drag();
    }

    private void Drag()
    {
        DrawFrame();
        DrawBox();
    }

    private void Click()
    {
        if (IsStart == true) return;
        SelectFrameTransform.position = Input;
        IsStart = true;
        Show();
        SelectedHideCircleLine();
        ClearSelectedBiologys();

    }

    private void ClearSelectedBiologys()
    {
        SelectBiologys.Clear();
    }

    internal void SelectedMoveTo(Vector3 vector3)
    {
        foreach (var item in SelectBiologys)
        {
            item.BiologyMovement.MoveTo(vector3);
        }
    }


    internal void ButtonUP()
    {
        if (IsStart == false) return;
        SelectedShowCircleLine();
        DrawBoxInitialize();
        Rest();
        Hide();
    }

    private void SelectedShowCircleLine()
    {
        foreach (var item in SelectBiologys)
        {
            item.CircleLine.Show();
        };
    }
    private void SelectedHideCircleLine()
    {
        foreach (var item in SelectBiologys)
        {
            item.CircleLine.Hide();
        };
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
        _SelectBoxVerts = new List<Vector3>(SelectBoxMesh.vertices);
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

    private void DrawBox()
    {
        OrthographicDrawBox();
        PerspectiveDrawBox();
    }

    private void DrawBoxInitialize()
    {
        // MeshCollider.sharedMesh = SelectBoxMesh;
        SelectBoxVerts = _SelectBoxVerts.ToArray();
        SelectBoxMesh.vertices = SelectBoxVerts;
        BoxCollider.size = Vector3.zero;
        BoxCollider.center = SelectBoxMesh.bounds.center;

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
        Ray eRay = Camera.main.ScreenPointToRay(Input);

        if (IsTinyDrawRange()) return;

        SelectBoxVerts[0] = SelectBoxVerts[8] = SelectBoxVerts[23] = SelectBoxTransform.InverseTransformPoint(sRay.origin);
        SelectBoxVerts[3] = SelectBoxVerts[9] = SelectBoxVerts[12] = SelectBoxTransform.InverseTransformPoint(sRay.origin + sRay.direction * 100);

        SelectBoxVerts[7] = SelectBoxVerts[18] = SelectBoxVerts[21] = SelectBoxTransform.InverseTransformPoint(eRay.origin);
        SelectBoxVerts[6] = SelectBoxVerts[14] = SelectBoxVerts[19] = SelectBoxTransform.InverseTransformPoint(eRay.origin + eRay.direction * 100);

        Ray tRay1 = Camera.main.ScreenPointToRay(new Vector3(SelectFrameTransform.position.x, Input.y, SelectFrameTransform.position.z));
        SelectBoxVerts[1] = SelectBoxVerts[17] = SelectBoxVerts[22] = SelectBoxTransform.InverseTransformPoint(tRay1.origin);
        SelectBoxVerts[2] = SelectBoxVerts[13] = SelectBoxVerts[16] = SelectBoxTransform.InverseTransformPoint(tRay1.origin + tRay1.direction * 100);

        Ray tRay2 = Camera.main.ScreenPointToRay(new Vector3(Input.x, SelectFrameTransform.position.y, SelectFrameTransform.position.z));
        SelectBoxVerts[4] = SelectBoxVerts[11] = SelectBoxVerts[20] = SelectBoxTransform.InverseTransformPoint(tRay2.origin);
        SelectBoxVerts[5] = SelectBoxVerts[10] = SelectBoxVerts[15] = SelectBoxTransform.InverseTransformPoint(tRay2.origin + tRay2.direction * 100);
        SelectBoxMesh.vertices = SelectBoxVerts;
    }
    private bool IsTinyDrawRange()
    {

        if (Mathf.Abs(SelectFrameTransform.position.x - Input.x) < 0.1f) return true;
        if (Mathf.Abs(SelectFrameTransform.position.y - Input.y) < 0.1f) return true;
        return false;
    }
    private bool IsSelectOtherUI()
    {
        if (EventSystem.current.currentSelectedGameObject == visualJoyStick.gameObject)
        {
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
        float _x = (Input.x - SelectFrameTransform.position.x);
        float _y = (SelectFrameTransform.position.y - Input.y);
        SelectFrameRectTransform.pivot = new Vector2(_x < 0 ? 1 : 0, _y > 0 ? 1 : 0);
        ResizeRect();

    }

    private void ResizeRect()
    {
        SelectFrameRectTransform.sizeDelta = new Vector2(Mathf.Abs(Input.x - SelectFrameTransform.position.x), Mathf.Abs(SelectFrameTransform.position.y - Input.y));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Biology>() == null) return;
        if (other.GetComponent<Biology>().Type != uFantasy.Enum.BiologyType.Player) return;
        Biology b = other.GetComponent<Biology>();
        Renderer r = b.transform.Find("Model/Model").GetComponent<Renderer>();
        SelectBiologys.Add(b);
        SelectBiologysRenderer.Add(r);
        HighlightsFXUpdate();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Biology>() == null) return;
        if (other.GetComponent<Biology>().Type != uFantasy.Enum.BiologyType.Player) return;
        Biology b = other.GetComponent<Biology>();
        Renderer r = b.transform.Find("Model/Model").GetComponent<Renderer>();
        SelectBiologysRenderer.Remove(r);
        HighlightsFXUpdate();

    }
    void HighlightsFXUpdate()
    {
        HighlightsFX.ClearOutlineData();
        HighlightsFX.AddRenderers(SelectBiologysRenderer);
    }
}
