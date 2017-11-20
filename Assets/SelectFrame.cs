using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectFrame : MonoBehaviour
{
    private bool IsStart;
    public Image Image { get; private set; }
    private RectTransform RectTransform;
    private Vector2 StartPos;

    // Use this for initialization
    void Start()
    {
        Image = GetComponent<UnityEngine.UI.Image>();
        RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsNotDraw()) return;
        DrawFrame();
        SelectBio();
    }

    private void SelectBio()
    {
        // fixme:選取生物功能
    }

    private bool IsNotDraw()
    {
        if (Input.GetMouseButton(0) == false || EventSystem.current.IsPointerOverGameObject())
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
        RectTransform.sizeDelta = Vector2.zero;
    }

    private void Hide()
    {
        Image.enabled = false;
    }

    private void Show()
    {
        Image.enabled = true;
    }

    private void DrawFrame()
    {
        if (IsStart == false)
        {
            StartPos = transform.position = Input.mousePosition;
            IsStart = true;
            Show();
            return;
        }

        if (IsStart == true)
        {
            float x = (Input.mousePosition.x - transform.position.x);
            float y = (transform.position.y - Input.mousePosition.y);

            if (x < 0 & y > 0)
            {
                RectTransform.pivot = new Vector2(1, 1);
                ResizeRect();
                return;
            }
            if (x > 0 & y < 0)
            {
                RectTransform.pivot = new Vector2(0, 0);
                ResizeRect();
                return;
            }
            if (x < 0 & y < 0)
            {
                RectTransform.pivot = new Vector2(1, 0);
                ResizeRect();
                return;
            }
            if (x > 0 & y > 0)
            {
                RectTransform.pivot = new Vector2(0, 1);
                ResizeRect();
                return;
            }
        }
    }

    private void ResizeRect()
    {
        RectTransform.sizeDelta = new Vector2(Mathf.Abs(Input.mousePosition.x - StartPos.x), Mathf.Abs(StartPos.y - Input.mousePosition.y));
    }
}
