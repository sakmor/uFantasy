using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyTest : MonoBehaviour
{

    public GameObject Target;

    // Use this for initialization
    void Start()
    {
        transform.LookAt(Target.transform.position, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
