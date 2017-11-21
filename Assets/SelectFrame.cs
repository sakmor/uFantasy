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
    private GameObject SelectBox;
    public float MagicNumber;

    // Use this for initialization
    void Start()
    {
        //sam: 魔術數字的來源是我用推算取得的
        //sam: 所以有可能在非電腦環境會有問題
        MagicNumber = 2048f / Screen.height;
        Image = GetComponent<UnityEngine.UI.Image>();
        RectTransform = GetComponent<RectTransform>();
        SelectBox = GameObject.Find("SelectBox");
        SelectBox.transform.SetParent(Camera.main.transform);
        SelectBox.transform.localPosition = Vector3.zero;
        SelectBox.transform.localEulerAngles = Vector3.zero;

        ViewportBounds = SelectBox.GetComponent<BoxCollider>().bounds;

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
        SelectBox.transform.position = Camera.main.ScreenToWorldPoint(transform.position);
        SelectBox.transform.localScale = new Vector3(RectTransform.sizeDelta.x * MagicNumber, RectTransform.sizeDelta.y * MagicNumber, Camera.main.farClipPlane);

        Debug.Log(ViewportBounds.Contains(
            Camera.main.WorldToViewportPoint(GameObject.Find("10001 騎士01").transform.position)));

        float x = (Input.mousePosition.x - transform.position.x);
        float y = (transform.position.y - Input.mousePosition.y);


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
