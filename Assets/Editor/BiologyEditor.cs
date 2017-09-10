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

        if (GUILayout.Button("載入資訊"))
        {
            Biology.LoadDB();
        }
    }
}