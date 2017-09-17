using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Biology))]
public class BiologyEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Biology Biology = (Biology)target;
        if (GUI.changed)
        {
            Biology.LoadDB();
        }
        if (GUILayout.Button("移除生物"))
        {
            Selection.activeGameObject = GameObject.Find("生物清單").gameObject;
            Biology.DestroyGameObject();
        }
        if (GUILayout.Button("載入資訊"))
        {
            Biology.LoadDB();
        }
    }


}