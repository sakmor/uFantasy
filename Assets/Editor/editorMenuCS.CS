using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class editorMenuCS : MonoBehaviour
{
    [MenuItem("[Menu]/清除重複")]
    static void CleanCube()
    {
        CUBE[] CUBE_list = FindObjectsOfType(typeof(CUBE)) as CUBE[];
        foreach (var item in CUBE_list)
        {
            item.name = item.transform.position.ToString("F0");
        }

    }
    [MenuItem("[Menu]/設定導航")]
    static void SetNavigationBake()
    {
        CUBE[] CUBE_list = FindObjectsOfType(typeof(CUBE)) as CUBE[];
        foreach (var item in CUBE_list)
        {
            if (item.transform.localPosition.y >= 1) GameObjectUtility.SetNavMeshArea(item.gameObject, 1);
        }

    }



}