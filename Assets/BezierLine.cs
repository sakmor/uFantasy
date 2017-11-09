//http://www.theappguruz.com/blog/bezier-curve-in-games
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class BezierLine : MonoBehaviour
{
    Transform LightStar;
    public Transform[] controlPoints;
    public LineRenderer lineRenderer;
    public bool drawIt = false;

    private int curveCount = 0;
    private int layerOrder = 0;
    private int SEGMENT_COUNT = 50;
    private float currentTime;
    public float duration, fadeOut;



    void Start()
    {
        duration = 1.25f;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerID = layerOrder;
        curveCount = (int)controlPoints.Length / 3;
        LightStar = transform.Find("LightStar");

        SetColor("Blue");
    }
    public void line2target(Transform Parent, Transform target)
    {
        Transform p0 = transform.Find("P0");

        p0.position = Parent.transform.position + Vector3.up * (5 + Mathf.Clamp(10 - Vector3.Distance(Parent.position, target.position), 0, 10));
        this.controlPoints[0] = Parent;
        this.controlPoints[1] = p0;
        this.controlPoints[2] = target;
        this.controlPoints[3] = target;
        drawIt = true;

    }
    public void line2target(Transform Parent, Transform target, string color)
    {
        line2target(Parent, target);
        SetColor(color);
    }
    public void closeLine()
    {
        this.controlPoints[2] = this.transform.parent.transform;
        this.controlPoints[3] = this.transform.parent.transform;
        drawIt = false;

    }

    public void SetGreen()
    {
        SetColor("Green");
    }
    public void SetYellow()
    {
        SetColor("Yellow");
    }
    public void SetRed()
    {
        SetColor("Red");
    }
    public void SetBlue()
    {
        SetColor("Blue");
    }

    public void SetColor(string c)
    {
        GetComponent<LineRenderer>().sharedMaterial = Resources.Load("Map/Materials/LightBeam" + c, typeof(Material)) as Material;
        LightStar.GetComponent<SpriteRenderer>().sharedMaterial = Resources.Load("Map/Materials/LightStar" + c, typeof(Material)) as Material;
    }
    void Update()
    {
        DrawCurve();
    }

    void DrawCurve()
    {
        if (drawIt == false) return;
        float t = 0;
        currentTime += Time.deltaTime;
        float pa = Easing.QuintEaseIn(currentTime, 0, 1, duration);

        for (int j = 0; j < curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                // 控制曲線繪圖進度(動畫)
                // Fixme:不易閱讀   
                t = i / (float)SEGMENT_COUNT;
                t = t >= pa ? pa : t;

                int nodeIndex = j * 3;
                Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position, controlPoints[nodeIndex + 1].position, controlPoints[nodeIndex + 2].position, controlPoints[nodeIndex + 3].position);
                lineRenderer.positionCount = (((j * SEGMENT_COUNT) + i));
                lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);
                transform.Find("LightStar").position = pixel + Vector3.up * 0.5f;
            }
        }

        if (t < 1) return;
        currentTime = 0;
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
