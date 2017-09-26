using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class editorMenuCS : MonoBehaviour
{

    static Dictionary<Vector3, GameObject> GetCleanMap()
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

        // 完全看不見的物件移除 
        //fixme: 會當機
        /*
        List<GameObject> DestoryList = new List<GameObject>();
        foreach (var item in NewMap)
        {
            if (NewMap.ContainsKey(item.Key + Vector3.up)
             && NewMap.ContainsKey(item.Key + Vector3.down)
             && NewMap.ContainsKey(item.Key + Vector3.left)
             && NewMap.ContainsKey(item.Key + Vector3.right))
            {
                var temp = item.Value;
                DestoryList.Add(item.Value);
                // DestroyImmediate(temp); //如果上下左右都有方塊擋住則視為看不見
            }
        }
        foreach (var gameObject in DestoryList)
        {
            DestroyImmediate(gameObject);
        }
 */
        return NewMap;

    }
    [MenuItem("[Menu]/場景整理 %f")]
    static void SetNavigationBake()
    {

        Dictionary<Vector3, GameObject> NewMap = GetCleanMap();
        foreach (var item in NewMap)
        {
            GameObjectUtility.SetNavMeshArea(item.Value, 0); //先預設為可走
            if (NewMap.ContainsKey(item.Key + Vector3.up) || NewMap.ContainsKey(item.Key + Vector3.up * 2))
            {
                GameObjectUtility.SetNavMeshArea(item.Value, 1); //如果方塊的正上方兩格有物體則不可走
            }
        }

        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();

    }



}