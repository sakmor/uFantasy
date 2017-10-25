using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotLine : MonoBehaviour
{

    private LineRenderer LineRenderer;
    private Transform Select_Arrow;
    // Use this for initialization
    private void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
        Select_Arrow = transform.Find("Select_Arrow").transform;
    }

    // Update is called once per frame
    public void DrawLine(Vector3[] postions)
    {
        LineRenderer.positionCount = postions.Length;
        LineRenderer.SetPositions(postions);
        float lineLength = GetLineLenght(postions);
        LineRenderer.material.SetFloat("_RepeatCount", lineLength * 2);
        Select_Arrow.position = postions[postions.Length - 1];
    }



    private float GetLineLenght(Vector3[] postions)
    {
        float dist = 0;
        for (var i = 0; i < postions.Length; i++)
        {
            int next_i = i == postions.Length - 1 ? i : i + 1;
            dist += Vector3.Distance(postions[i], postions[next_i]);
        }
        return dist;
    }

    internal void DrawLineStop()
    {
        Select_Arrow.position = Vector3.down * 99;
        LineRenderer.positionCount = 0;
    }
}
