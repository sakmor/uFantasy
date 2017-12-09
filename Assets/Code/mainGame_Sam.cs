using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mainGame_Sam : MonoBehaviour
{
    public float ResolutionScale = 0.5f, ButtonProcessTime;
    public SelectUnit SelectUnit;
    public Biology[] Biologys; //紀錄場景上所有的生物
    private DotLine DotLine;
    private enum InputState { Down, Hold, Up, None }
    private mainGame_Sam.InputState CurrentInputState, LastInputState;
    public bool IsDrag;
    private float DragDist = 30;

    private Vector3 InputPos, _InputPos;

    private void Awake()
    {
        CurrentInputState = LastInputState = InputState.None;
        Biologys = (Biology[])FindObjectsOfType(typeof(Biology));
    }
    // Use this for initialization
    private void Start()
    {
        DotLine = GameObject.Find("Line").GetComponent<DotLine>();
    }

    // Update is called once per frame
    private void Update()
    {
        InputProcess();
        ButtonProcess();
    }

    private void ButtonProcess()
    {
        if (!Input.anyKey) { ButtonProcessTime = 1; return; }
        ButtonProcessTime += Time.deltaTime * 2;
        if (Input.GetKey("w")) Camera.main.GetComponent<CameraMovement>().OffsetCamTarget(Vector2.up, ButtonProcessTime * Time.deltaTime);
        if (Input.GetKey("a")) Camera.main.GetComponent<CameraMovement>().OffsetCamTarget(Vector2.left, ButtonProcessTime * Time.deltaTime);
        if (Input.GetKey("s")) Camera.main.GetComponent<CameraMovement>().OffsetCamTarget(Vector2.down, ButtonProcessTime * Time.deltaTime);
        if (Input.GetKey("d")) Camera.main.GetComponent<CameraMovement>().OffsetCamTarget(Vector2.right, ButtonProcessTime * Time.deltaTime);
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
        InputPosUpdate();
        DragStateUpdate();
        InputStateUpdate();

        InputNone();
        InputHold();
        InputDrag();
        InputUp();
        InputDragUp();
        InputDown();
    }

    public Vector3 GetInputPos()
    {
        return InputPos;
    }

    private void InputDragUp()
    {
        if (CurrentInputState != InputState.Up || IsDrag == false) return;
        SelectUnit.InputDragUp(GetRayCastHitPos());
    }

    private void InputDrag()
    {
        if (CurrentInputState != InputState.Hold || IsDrag == false) return;
        SelectUnit.InputDrag();
    }

    private void DragStateUpdate()
    {
        if (CurrentInputState == InputState.Down) _InputPos = InputPos;
        if (CurrentInputState == InputState.Up) IsDrag = false;
        if (CurrentInputState == InputState.Hold && Vector3.Distance(InputPos, _InputPos) > DragDist) IsDrag = true;
    }

    private void InputStateUpdate()
    {
        if (IsTouch() == true)
        {
            if (LastInputState == InputState.None || LastInputState == InputState.Up) CurrentInputState = InputState.Down;
            if (LastInputState == InputState.Hold || LastInputState == InputState.Down) CurrentInputState = InputState.Hold;
        }
        if (IsTouch() == false)
        {
            if (LastInputState == InputState.Hold || LastInputState == InputState.Down) CurrentInputState = InputState.Up;
            if (LastInputState == InputState.None || LastInputState == InputState.Up) CurrentInputState = InputState.None;
        }
        LastInputState = CurrentInputState;
    }

    private void InputDown()
    {
        if (CurrentInputState == InputState.Down)
            SelectUnit.InputDown();
    }
    private void InputNone()
    {
        if (CurrentInputState != InputState.None) return;
        SelectUnit.InputNone();

    }
    private void InputUp()
    {
        if (CurrentInputState != InputState.Up || IsDrag || false) return;

        SelectUnit.InputUp(GetRayCastHitPos());
        DotLine.DrawLineStop();
    }
    private void InputHold()
    {
        if (CurrentInputState != InputState.Hold && IsDrag == true) return;
        SelectUnit.InputHold();
    }


    private Vector3 GetRayCastHitPos()
    {
        //由攝影機產生一條射線
        Ray ray = Camera.main.ScreenPointToRay(InputPos);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (var item in hits)
        {
            return item.point;
        }
        return Vector3.negativeInfinity;
    }

    private bool IsTouch()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (Input.GetMouseButton(0)) return true;
        }
        if (Application.platform == RuntimePlatform.Android) if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)) return true;

        return false;
    }

    public void InputPosUpdate()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WebGLPlayer)
        {
            InputPos = Input.mousePosition;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            InputPos = Input.GetTouch(0).position;
        }
    }


}
