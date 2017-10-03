//
//Filename: maxCamera.cs
//
// original: http://www.unifycommunity.com/wiki/index.php?title=MouseOrbitZoom
//
// --01-18-2010 - create temporary target, if none supplied at start

using UnityEngine;
using System.Collections;


[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class CameraMovement : MonoBehaviour
{

    public Vector3 targetOffset;
    public float distance = 3.0f;
    public float maxDistance = 5.0f;
    public float minDistance = 1.6f;
    public int zoomRate = 40;
    public float panSpeed = 0.3f;
    public float zoomDampening = 1.5f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    private GameObject go;
    private Transform target;
    private float smoothTime = 0.5F;
    private Vector3 velocity = Vector3.zero;
    private bool isMove = false;

    IEnumerator moveCoroutine;
    void Start() { Init(); }
    void OnEnable() { Init(); }

    public void Init()
    {


        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!go) go = new GameObject("Cam Target");

        go.transform.position = transform.position + (transform.forward * distance);
        target = go.transform;

        currentDistance = distance;
        desiredDistance = distance;

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);

        TargetLeader();
    }

    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void LateUpdate()
    {

        if (Input.GetMouseButton(0))
        {
            StopCoroutine(moveCoroutine);
            //grab the rotation of the camera so we can move in a psuedo local XY space
            target.rotation = transform.rotation;
            target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed);
            target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);
        }

        ////////Orbit Position
        // affect the desired Zoom distance if we roll the scrollwheel
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

        GetComponent<Camera>().orthographicSize = currentDistance;

        // calculate position based on the new currentDistance 
        position = target.position - (rotation * Vector3.forward * 100 + targetOffset);
        transform.position = position;

        if (Input.GetMouseButtonUp(0))
        {
            TargetLeader();
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void TargetLeader()
    {
        moveto(GameObject.Find("10001 騎士01").transform);
    }

    public void SetLeader()
    {

    }
    public void moveto(Transform pos)
    {
        //moveCoroutine
        moveCoroutine = moving(pos);

        //停止正在進行的 moveCoroutine ，如果沒有預先指定（即14行）會無法通過編譯
        StopCoroutine(moveCoroutine);

        //指定 moveCoroutine 使用的迭代器與參數 --- IEnumberator moving(vector3,GameObject)
        moveCoroutine = moving(pos);

        //執行 moveCoroutine
        StartCoroutine(moveCoroutine);
    }

    IEnumerator moving(Transform pos)
    {
        //這段程式碼只執行一次
        float dist = Mathf.Infinity;
        isMove = true;

        // 當「物件位置」與「目的位置」距離差距超過0.1f距離單位以上時，while內的程式買將重複執行
        while (dist > 0.00125f && isMove == true)
        {
            //計算與目標之間的距離差並儲存到dist
            dist = Vector3.Distance(target.transform.position, pos.position);
            //Vector3.SmoothDamp (起始位置、目標位置、當前速度、抵達時間)
            target.transform.position = Vector3.SmoothDamp(target.transform.position, pos.position, ref velocity, smoothTime);

            yield return null;
        }

        // 當「物件位置」與「目的位置」距離差距低於0.1f距離單位以上時，下面程式碼會執行後跳出 moving 迭代器
        target.transform.position = pos.position;
        isMove = false;


    }
}