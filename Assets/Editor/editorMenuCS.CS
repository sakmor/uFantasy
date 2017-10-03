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

    [MenuItem("我的工具/場景整理 %1")]
    static void SetNavigationBake()
    {
        exeTime = Time.realtimeSinceStartup;

        GetCleanMap();
        SetEveryCubeWalkable();
        BuildNavMesh();
        RemoveHideCube();

        Debug.Log("整理完畢 " + (Time.realtimeSinceStartup - exeTime).ToString("F2"));

    }
    [MenuItem("我的工具/所有生物衝向目標 %2")]
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

        Camera.main.GetComponent<CameraMovement>().TargetLeader();

    }
    [MenuItem("我的工具/AnimatorController %3")]
    static void SetModelAnimator()
    {
        var biologys = Resources.LoadAll("Biology/", typeof(GameObject));
        foreach (var i in biologys)
        {
            SetAnimator(i.name);
        }
    }
    static void SetAnimator(string modelName)
    {
        if (modelName == "Empty" || modelName == "Shadow") return;
        Dictionary<string, Motion> Motions = GetMotions(modelName);
        exeTime = Time.realtimeSinceStartup;
        UnityEditor.Animations.AnimatorController AnimatorController = Resources.Load("Biology/Motions/" + modelName, typeof(UnityEditor.Animations.AnimatorController)) as UnityEditor.Animations.AnimatorController;
        var states = AnimatorController.layers[0].stateMachine.states;
        foreach (var state in states)
        {
            Debug.Log(state.state.name);
            if (Motions.ContainsKey(modelName + "_" + state.state.name))
            {
                state.state.motion = Motions[modelName + "_" + state.state.name];
            }
        }
        Debug.Log("AnimatorController 整理完畢 " + (Time.realtimeSinceStartup - exeTime).ToString("F2"));
    }
    static Dictionary<string, Motion> GetMotions(string modelName)
    {
        var MotionArray = Resources.LoadAll("Biology/" + modelName, typeof(AnimationClip));

        Dictionary<string, Motion> Motions = new Dictionary<string, Motion>();
        foreach (var i in MotionArray)
        {
            Motions.Add(i.name, i as Motion);
        }
        return Motions;
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