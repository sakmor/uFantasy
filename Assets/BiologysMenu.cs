using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologysMenu : MonoBehaviour
{
    [HideInInspector] public Component[] Biologys;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateBiologysList()
    {
        Biologys = GetComponentsInChildren<Biology>();
    }


}
