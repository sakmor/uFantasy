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
    private int SEGMENT_COUNT = 25;
    private float currentTime, currentTimeAlpha;
    public float duration, alpha;
    private Color Color;
    private Transform p0;

    void Start()
    {
        duration = 1.25f;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerID = layerOrder;
        curveCount = (int)controlPoints.Length / 3;
        LightStar = transform.Find("LightStar");

        SetColor("Blue");
    }
    public void Line2Target(Transform Parent, Transform Target)
    {

        if (drawIt == true) return;
        p0 = transform.Find("P0");

        this.controlPoints[0] = Parent;
        this.controlPoints[1] = p0;
        this.controlPoints[2] = Target;
        this.controlPoints[3] = Target;
        drawIt = true;

    }
    public void Line2Target(Transform Parent, Transform target, string color)
    {
        Line2Target(Parent, target);
        SetColor(color);
    }

    public void SetColor(string c)
    {
        lineRenderer.sharedMaterial = Instantiate(Resources.Load("Map/Materials/LightBeam" + c, typeof(Material)) as Material);
        LightStar.GetComponent<SpriteRenderer>().sharedMaterial = Instantiate(Resources.Load("Map/Materials/LightStar" + c, typeof(Material)) as Material);
        Color = lineRenderer.material.GetColor("_TintColor");
    }
    void Update()
    {
        DrawCurve();
    }

    void DrawCurve()
    {
        if (drawIt == false) return;
        lineRenderer.material.SetColor("_TintColor", new Color(Color.r, Color.g, Color.b, 0.5f));
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

        p0.position = controlPoints[0].position + Vector3.up * (5 + Mathf.Clamp(10 - Vector3.Distance(controlPoints[0].position, controlPoints[3].position), 0, 10));
        p0.transform.position -= Vector3.up * 1.5f * currentTime;

        if (t < 1) return;
        currentTimeAlpha += Time.deltaTime;
        alpha = 0.5f - currentTimeAlpha;
        lineRenderer.material.SetColor("_TintColor", new Color(Color.r, Color.g, Color.b, alpha));


        if (alpha > 0f) return;
        currentTimeAlpha = 0;
        currentTime = 0;
        drawIt = false;


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
