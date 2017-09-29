using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class editorMenuCS : MonoBehaviour
{
    public static float exeTime;
    public static Dictionary<Vector3, GameObject> CleanMap;

    [MenuItem("我的工具/場景整理 %f")]
    static void SetNavigationBake()
    {
        exeTime = Time.realtimeSinceStartup;

        GetCleanMap();
        SetEveryCubeWalkable();
        BuildNavMesh();
        RemoveHideCube();

        Debug.Log("整理完畢 " + (Time.realtimeSinceStartup - exeTime).ToString("F2"));

    }
    [MenuItem("我的工具/所有生物衝向目標 %F1")]
    static void GoGoalPos()
    {
        if (Application.isPlaying == false)
        {
            EditorUtility.DisplayDialog("System", "請在撥放模式下使用", "知道了");
            return;
        }

        Biology[] Biology_List = FindObjectsOfType(typeof(Biology)) as Biology[];
        Vector3 pos;
        try
        {
            pos = GameObject.Find("GoalPos").transform.position;
        }
        catch
        {
            EditorUtility.DisplayDialog("System", "找不到GoalPos", "好");
            return;
        }

        foreach (var Biology in Biology_List)
        {
            Biology.BiologyMovement.MoveTo(pos);
        }

    }
    static void GetCleanMap()
    {
        CUBE[] CUBE_list = FindObjectsOfType(typeof(CUBE)) as CUBE[];
        Dictionary<Vector3, GameObject> NewMap = new Dictionary<Vector3, GameObject>();

        // 消除重複取得無重複場景
        foreach (var item in CUBE_list)
        {
            item.name = item.transform.position.ToString("F0");
            if (NewMap.ContainsKey(item.transform.position))
            {
                DestroyImmediate(item.gameObject);
            }
            else
            {
                NewMap.Add(item.transform.position, item.gameObject);
            }
        }
        CleanMap = NewMap;

    }
    private static void RemoveHideCube()
    {

        List<GameObject> DestoryList = new List<GameObject>();
        foreach (var item in CleanMap)
        {
            Vector3 pos = item.Key;
            if (CleanMap.ContainsKey(pos + new Vector3(1, 0, 0))
             && CleanMap.ContainsKey(pos + new Vector3(-1, 0, 0))
             && CleanMap.ContainsKey(pos + new Vector3(0, 1, 0))
             && CleanMap.ContainsKey(pos + new Vector3(0, -1, 0))
             && CleanMap.ContainsKey(pos + new Vector3(0, 0, 1))
             && CleanMap.ContainsKey(pos + new Vector3(0, 0, -1)))
            {
                var temp = item.Value;
                DestoryList.Add(item.Value);
            }
        }
        for (var i = 0; i < DestoryList.Count; i++)
        {
            CleanMap.Remove(DestoryList[i].transform.position);
            DestroyImmediate(DestoryList[i]);
        }
    }

    private static void BuildNavMesh()
    {
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }

    private static void SetEveryCubeWalkable()
    {
        foreach (var item in CleanMap)
        {
            GameObjectUtility.SetNavMeshArea(item.Value, 0);
        }
    }
}