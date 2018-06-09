using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyTest : MonoBehaviour
{

    public GameObject Target;
    public Quaternion targetRotation;
    public float Angle;

    // Use this for initialization
    void Start()
    {
        transform.LookAt(Target.transform.position, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        Angle = Quaternion.Angle(transform.rotation, targetRotation);

    }
}
