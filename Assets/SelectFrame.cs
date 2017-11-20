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
    private Bounds ViewportBounds;

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
        ViewportBounds = GetViewportBounds();

        Debug.Log(ViewportBounds.Contains(
            Camera.main.WorldToViewportPoint(GameObject.Find("10001 騎士01").transform.position)));
        // OnDrawGizmosSelected();

    }

    public Bounds GetViewportBounds()
    {
        var v1 = Camera.main.ScreenToViewportPoint(transform.position);
        var v2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = Camera.main.nearClipPlane;
        max.z = Camera.main.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);

        return bounds;
    }
    private bool IsNotDraw()
    {

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
            transform.position = Input.mousePosition;
            IsStart = true;
            Show();
            return;
        }

        float x = (Input.mousePosition.x - transform.position.x);
        float y = (transform.position.y - Input.mousePosition.y);
        RectTransform.pivot = new Vector2(x < 0 ? 1 : 0, y > 0 ? 1 : 0);
        ResizeRect();

    }

    private void ResizeRect()
    {
        RectTransform.sizeDelta = new Vector2(Mathf.Abs(Input.mousePosition.x - transform.position.x), Mathf.Abs(transform.position.y - Input.mousePosition.y));
    }
}
