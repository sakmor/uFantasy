using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Biology>() == null) return;
        if (other.transform.GetComponent<Biology>().Type != uFantasy.Enum.BiologyType.Player) return;
        Biology biology = other.transform.GetComponent<Biology>();
        biology.HpUI.Show();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<Biology>() == null) return;
        if (other.transform.GetComponent<Biology>().Type != uFantasy.Enum.BiologyType.Player) return;
        Biology biology = other.transform.GetComponent<Biology>();
        biology.HpUI.Hide();
    }
}
