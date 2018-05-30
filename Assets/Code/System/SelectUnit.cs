using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectUnit : MonoBehaviour
{
	private mainGame_Sam mainGame;
	public Image SelectFrameImage { get; private set; }
	private Vector2 Input { get { return mainGame.GetInputPos(); } set { } }
	private Animator TouchDownCursorAnimatior;
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
	[SerializeField] internal List<Biology> SelectBiologys;
	private List<Biology> _SelectBiologys = new List<Biology>();
	private List<Renderer> SelectBiologysRenderer = new List<Renderer>();
	private float Depth;
	private Toggle DragModelToggle;
	public bool IsPcControl = true;
	public bool IsDragModel;
	private CameraMovement CameraMovement;

	// Use this for initialization
	void Start()
	{
		CameraMovement = Camera.main.GetComponent<CameraMovement>();
		mainGame = GameObject.Find("mainGame").GetComponent<mainGame_Sam>(); //fixme:應該減少使用GameObject.find
		SelectBoxTransform = transform.Find("SelectBox");
		HighlightsFX = Camera.main.GetComponent<HighlightsFX>();
		Depth = Camera.main.farClipPlane;
		IsDragModel = false;
		if (IsPcControl) IsDragModel = true;
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
	internal void InputNone()
	{

	}
	internal void InputHold()
	{

	}
	internal void InputUp(RaycastHit raycastHit)
	{
		if (IsSelectOtherUI()) return;
	}
	internal void InputRightKeyUp(RaycastHit raycastHit)
	{
		if (IsSelectOtherUI()) return;
		MoveBio(raycastHit);
	}
	internal void InputLeftKeyUp(RaycastHit raycastHit)
	{
		if (IsSelectOtherUI()) return;
		SelectBio(raycastHit);
	}
	private void MoveBio(RaycastHit raycastHit)
	{
		if (raycastHit.transform.tag == "Terrain") TerrainHit(raycastHit.point);
	}
	private void SelectBio(RaycastHit raycastHit)
	{
		if (raycastHit.transform.tag == "Player") SetSingleBiologySelected(raycastHit.transform.GetComponent<Biology>());
	}
	internal void InputDown()
	{
		if (IsSelectOtherUI()) return;
		SelectFrameTransform.position = Input;
		CameraMovement.CameraMovementInit(Input);
	}

	internal void InputRightKeyUp()
	{
		throw new NotImplementedException();
	}

	internal void InputDrag()
	{
		if (IsSelectOtherUI()) return;
		DrawFrameStart();
	}
	internal void InputDragUp()
	{
		SelectBiologysUpdate();
		SelectBiologysShowCircleLine();
		DrawBoxInitialize();
		Rest();
		Hide();
		SetDragModelOff();
	}
	private void DragCameraMovement()
	{
		CameraMovement.CameraMovementDrag(Input);
	}

	private void DrawFrameStart()
	{
		Show();
		DrawFrame();
		DrawBox();
	}
	private void TerrainHit(Vector3 pos)
	{
		SelectBiologyMoveTo(pos);
	}


	private void SetSingleBiologySelected(Biology biology)
	{
		if (SelectBiologys == null) SelectBiologys = new List<Biology>();
		SelectBiologysHideCircleLine();
		SelectBiologys.Clear();
		SelectBiologys.Add(biology);
		biology.CircleLine.Show();
	}


	private void ClearLastSelectBiology()
	{
		SelectBiologysHideCircleLine();
		SelectBiologysClear();
	}
	private void SelectBiologysUpdate()
	{
		if (_SelectBiologys.Count == 0) return;
		SelectBiologys = new List<Biology>(_SelectBiologys);
	}
	private void SelectBiologysClear()
	{
		if (SelectBiologys == null) return;
		SelectBiologys.Clear();
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
		SelectBoxVerts = _SelectBoxVerts.ToArray();

		PerspectiveDrawBoxInitialize();
		OrthographicDrawBoxInitialize();
	}

	private void PerspectiveDrawBoxInitialize()
	{
		if (Camera.main.orthographic == true) return;
		SelectBoxMesh.vertices = _SelectBoxVerts.ToArray();
		MeshCollider.sharedMesh = SelectBoxMesh;
	}

	private void OrthographicDrawBoxInitialize()
	{
		if (Camera.main.orthographic == false) return;
		SelectBoxMesh.vertices = SelectBoxVerts;
		BoxCollider.size = Vector3.zero;
		BoxCollider.center = SelectBoxMesh.bounds.center;
	}
	private void PerspectiveDrawBox()
	{
		if (Camera.main.orthographic == true) return;
		if (IsTinyDrawRange()) return;
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

		SelectBoxVerts[0] = SelectBoxVerts[8] = SelectBoxVerts[23] = SelectBoxTransform.InverseTransformPoint(sRay.origin);
		SelectBoxVerts[3] = SelectBoxVerts[9] = SelectBoxVerts[12] = SelectBoxTransform.InverseTransformPoint(sRay.origin + sRay.direction * Depth);

		SelectBoxVerts[7] = SelectBoxVerts[18] = SelectBoxVerts[21] = SelectBoxTransform.InverseTransformPoint(eRay.origin);
		SelectBoxVerts[6] = SelectBoxVerts[14] = SelectBoxVerts[19] = SelectBoxTransform.InverseTransformPoint(eRay.origin + eRay.direction * Depth);

		Ray tRay1 = Camera.main.ScreenPointToRay(new Vector3(SelectFrameTransform.position.x, Input.y, SelectFrameTransform.position.z));
		SelectBoxVerts[1] = SelectBoxVerts[17] = SelectBoxVerts[22] = SelectBoxTransform.InverseTransformPoint(tRay1.origin);
		SelectBoxVerts[2] = SelectBoxVerts[13] = SelectBoxVerts[16] = SelectBoxTransform.InverseTransformPoint(tRay1.origin + tRay1.direction * Depth);

		Ray tRay2 = Camera.main.ScreenPointToRay(new Vector3(Input.x, SelectFrameTransform.position.y, SelectFrameTransform.position.z));
		SelectBoxVerts[4] = SelectBoxVerts[11] = SelectBoxVerts[20] = SelectBoxTransform.InverseTransformPoint(tRay2.origin);
		SelectBoxVerts[5] = SelectBoxVerts[10] = SelectBoxVerts[15] = SelectBoxTransform.InverseTransformPoint(tRay2.origin + tRay2.direction * Depth);
		SelectBoxMesh.vertices = SelectBoxVerts;
	}
	private bool IsTinyDrawRange()
	{
		if (Mathf.Abs(SelectFrameTransform.position.x - Input.x) < 5f) return true;
		if (Mathf.Abs(SelectFrameTransform.position.y - Input.y) < 5f) return true;
		return false;
	}
	private bool IsSelectOtherUI()
	{
		if (EventSystem.current.currentSelectedGameObject)
		{
			return true;
		}
		return false;
	}

	public void AllPlayerSelected()
	{
		if (SelectBiologys == null) SelectBiologys = new List<Biology>();
		SelectBiologysHideCircleLine();
		SelectBiologys.Clear();
		var AllBiologys = GameObject.FindGameObjectsWithTag("Player");

		foreach (var b in AllBiologys)
		{
			Biology biology = b.GetComponent<Biology>();
			if (biology.Type != uFantasy.Enum.BiologyType.Player) continue;
			SelectBiologys.Add(biology);
			biology.CircleLine.Show();
		}
	}

	private void Rest()
	{
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

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Biology>() == null) return;
		if (other.GetComponent<Biology>().Type != uFantasy.Enum.BiologyType.Player) return;
		if (other.GetComponent<Biology>().BiologyAttr.Hp <= 0) return;
		ClearLastSelectBiology();
		Biology b = other.GetComponent<Biology>();
		_SelectBiologysAdd(b);
	}
	private void OnTriggerExit(Collider other)
	{

		if (other.GetComponent<Biology>() == null) return;
		if (other.GetComponent<Biology>().Type != uFantasy.Enum.BiologyType.Player) return;
		Biology b = other.GetComponent<Biology>();
		_SelectBiologysRemove(b);
	}
	void HighlightsFXUpdate()
	{
		HighlightsFX.ClearOutlineData();
		HighlightsFX.AddRenderers(SelectBiologysRenderer);
	}

	private void _SelectBiologysAdd(Biology b)
	{
		Renderer r = b.transform.Find("Model/Model").GetComponent<Renderer>();
		SelectBiologysRenderer.Add(r);
		_SelectBiologys.Add(b);
		HighlightsFXUpdate();
	}
	internal void _SelectBiologysRemove(Biology b)
	{
		Renderer r = b.transform.Find("Model/Model").GetComponent<Renderer>();
		SelectBiologysRenderer.Remove(r);
		_SelectBiologys.Remove(b);
		HighlightsFXUpdate();
	}

	void SelectBiologysShowCircleLine()
	{
		if (SelectBiologys == null) return;
		foreach (var b in SelectBiologys)
		{
			b.CircleLine.Show();
		}
	}
	private void SelectBiologysHideCircleLine()
	{
		if (SelectBiologys == null) return;
		foreach (var b in SelectBiologys)
		{
			b.CircleLine.Hide();
		}
	}

	private Vector3 GetSelectBiologysCenter()
	{
		float x = 0, y = 0, z = 0;
		float count = SelectBiologys.Count;
		for (int i = 0; i < SelectBiologys.Count; i++)
		{
			Biology b = SelectBiologys[i];
			x += b.transform.position.x;
			y += b.transform.position.y;
			z += b.transform.position.z;
		}
		x /= count; y /= count; z /= count;
		return new Vector3(x, y, z);
	}

	private Vector3[] GetSelectBiologysToCenters()
	{
		Vector3 center = GetSelectBiologysCenter();
		Vector3[] BiologysToCenters = new Vector3[SelectBiologys.Count];
		for (int i = 0; i < SelectBiologys.Count; i++)
		{
			Biology b = SelectBiologys[i];
			BiologysToCenters[i] = b.transform.position - center;
		}
		return BiologysToCenters;
	}

	private Vector3[] GetSelectBiologyGoal(Vector3 inputPos)
	{
		Vector3[] SelectBiologyGoal = new Vector3[SelectBiologys.Count]; ;
		Vector3[] BiologysToCenters = GetSelectBiologysToCenters();
		for (int i = 0; i < SelectBiologys.Count; i++)
		{
			SelectBiologyGoal[i] = BiologysToCenters[i] + inputPos;
		}
		return SelectBiologyGoal;
	}

	private Vector3[] GetSelectBiologyFormationl(Vector3 inputPos)
	{
		Vector3[] SelectBiologyGoal = new Vector3[SelectBiologys.Count]; ;
		Vector3[] BiologysToCenters = GetSelectBiologysToCenters();
		for (int i = 0; i < SelectBiologys.Count; i++)
		{
			SelectBiologyGoal[i] = BiologysToCenters[i] + inputPos;
		}
		return SelectBiologyGoal;
	}

	private void SelectBiologyMoveTo(Vector3 inputPos)
	{
		if (SelectBiologys == null) return;
		Vector3[] SelectBiologyGoal = GetSelectBiologyGoal(inputPos);
		for (int i = 0; i < SelectBiologys.Count; i++)
		{
			Vector3 goal = SelectBiologyGoal[i];
			SelectBiologys[i].BiologyMovement.InputMoveto(goal);
		}
	}
	public void DragModelChange(Toggle t)
	{
		DragModelToggle = t;
		IsDragModel = t.isOn;
	}

	public void SetDragModelOff()
	{
		IsDragModel = false;
		if (IsPcControl) IsDragModel = true;
		if (DragModelToggle) DragModelToggle.isOn = false;
	}

}
