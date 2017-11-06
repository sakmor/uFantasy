using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class BezierLine : MonoBehaviour
{
    public Transform[] controlPoints;
    public LineRenderer lineRenderer;
    public bool drawIt = false;

    private int curveCount = 0;
    private int layerOrder = 0;
    private int SEGMENT_COUNT = 50;
    private float startTime;
    public float duration, linepapa, fadeOut;



    void Start()
    {
        linepapa = 0;
        duration = 0.25f;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerID = layerOrder;
        curveCount = (int)controlPoints.Length / 3;
        startTime = Time.time;
    }
    public void line2target(Transform Parent, Transform target)
    {
        transform.Find("P0").localPosition = new Vector3(0, 10, 0);
        this.controlPoints[0] = Parent;
        this.controlPoints[1] = transform.Find("P0");
        this.controlPoints[2] = target;
        this.controlPoints[3] = target;
        drawIt = true;
        linepapa = 0;
        startTime = Time.time;

    }
    public void closeLine()
    {
        this.controlPoints[2] = this.transform.parent.transform;
        this.controlPoints[3] = this.transform.parent.transform;
        drawIt = false;

    }


    void Update()
    {
        // print(Time.deltaTime);
        if (drawIt)
        {
            DrawCurve();
        }

    }

    void DrawCurve()
    {

        for (int j = 0; j < curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;

                // if (linepapa <= 0.9)
                // {

                //     linepapa += 0.001f;// MathS.easeInQuad(Time.time - startTime, 0, 1, duration);

                //     if (t > linepapa)
                //     {
                //         t = linepapa;
                //     }
                //     fadeOut = 1;
                // }
                // else
                // {
                //     // fadeOut -= Time.deltaTime * 0.03f;
                //     // if (fadeOut > 0)
                //     // {
                //     //     rend.material.SetColor("_Color", new Color(1, 1, 1, fadeOut));
                //     // }
                //     // else
                //     // {
                //     //     drawIt = false;

                //     // }
                // }
                int nodeIndex = j * 3;
                Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position, controlPoints[nodeIndex + 1].position, controlPoints[nodeIndex + 2].position, controlPoints[nodeIndex + 3].position);
                lineRenderer.positionCount = (j * SEGMENT_COUNT) + i;
                lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);

            }

        }
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
